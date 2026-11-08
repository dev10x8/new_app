#!/usr/bin/env python3
"""
mdf_users.py - locate and read/write user rows (incl. the login PIN) directly
inside a SQL Server .MDF file, without attaching the database.

Background
----------
In Madhushala Pro the login "PIN" is the `users.password` column stored as
plain text (no hashing). The admin account is:

    usercode = A00001, username = Admin, accesslevel = Admin, password = <pin>

This module parses the data page that holds that row and can rewrite the
password in place while fixing the page checksum (see PAGE CHECKSUM below).

Row layout (validated against the shipped MDFs)
-----------------------------------------------
A SQL Server data page is 8192 bytes; the page header is 96 bytes and the slot
array lives at the end of the page (2 bytes per slot: offset, length).

Within the row, the four variable-length columns are stored contiguously:

    usercode(6) + username + accesslevel + password

The row header ends with the variable-column end-offsets (2 bytes each,
little-endian, measured from the start of the record). This module finds the
`A00001` marker, reads those offsets, and slices the columns from them, so it
does not hard-code username/accesslevel/password lengths.

PAGE CHECKSUM
-------------
Every SQL Server page carries a checksum at byte offset 60. Editing page bytes
without recomputing it makes the file look corrupt (DBCC CHECKDB fails / the
engine refuses to attach). The algorithm used by these MDFs is the classic
XOR variant:

    PAGE_SIZE = 8192, SECTORS = 16, ELEMENTS = 128, SEED = 15

    checksum(page):
        dwords = little-endian uint32 view of the 8192-byte page (2048 words)
        # sector 0: XOR all 128 dwords except index 15 (the checksum slot)
        acc = XOR of dwords[0..127] with index 15 skipped
        ck  = rol(acc, 15)
        for s in 1..15:
            acc = XOR of dwords[s*128 .. s*128+127]
            ck ^= rol(acc, 15 - s)
        return ck
"""

from __future__ import annotations

import struct
import sys
from dataclasses import dataclass
from pathlib import Path

PAGE_SIZE = 8192
CHECKSUM_OFFSET = 60
MASK32 = 0xFFFFFFFF
SECTORS = 16
ELEMENTS = 128
SEED = 15

ADMIN_USERCODE = b"A00001"


def _rol(value: int, bits: int) -> int:
    bits &= 31
    if bits == 0:
        return value & MASK32
    return ((value << bits) | (value >> (32 - bits))) & MASK32


def page_checksum(page: bytes) -> int:
    """Return the computed checksum for one 8192-byte page."""
    if len(page) != PAGE_SIZE:
        raise ValueError("page must be exactly 8192 bytes")
    words = struct.unpack("<2048I", page)
    acc = 0
    for i in range(ELEMENTS):
        if i != 15:  # index 15 holds the stored checksum
            acc ^= words[i]
    ck = _rol(acc, SEED)
    for sector in range(1, SECTORS):
        acc = 0
        base = sector * ELEMENTS
        for j in range(ELEMENTS):
            acc ^= words[base + j]
        ck ^= _rol(acc, SEED - sector)
    return ck


@dataclass
class UserRow:
    """One row parsed out of the users table."""

    page_no: int
    record_start: int          # absolute file offset of the record
    column_ends: list[int]     # variable-column end offsets (from record start)
    columns: list[bytes]       # decoded column values

    @property
    def usercode(self) -> str:
        return self.columns[0].decode("latin-1")

    @property
    def username(self) -> str:
        return self.columns[1].decode("latin-1")

    @property
    def accesslevel(self) -> str:
        return self.columns[2].decode("latin-1")

    @property
    def password(self) -> str:
        return self.columns[3].decode("latin-1")

    @property
    def password_span(self) -> tuple[int, int]:
        """(start, end) absolute offsets of the password bytes in the file."""
        start = self.record_start + (self.column_ends[2] if len(self.column_ends) > 2 else 0)
        end = self.record_start + self.column_ends[3]
        return start, end


def _read_offsets(data: bytes, data_start: int, ncols: int) -> list[int]:
    """Read `ncols` little-endian end-offsets stored just before `data_start`."""
    raw = data[data_start - 2 * ncols:data_start]
    return list(struct.unpack("<%dH" % ncols, raw))


def _plausible(data: bytes, data_start: int, ncols: int) -> list[int] | None:
    """Return the offset array if it is self-consistent, else None."""
    if data_start - 2 * ncols < 0:
        return None
    offsets = _read_offsets(data, data_start, ncols)
    # strictly increasing, positive
    if any(offsets[i] >= offsets[i + 1] for i in range(len(offsets) - 1)):
        return None
    if offsets[0] <= 0:
        return None
    # record starts (offsets[0] - len(usercode)) bytes before the data
    record_start = data_start - (offsets[0] - len(ADMIN_USERCODE))
    if record_start < 0:
        return None
    # header byte 4..6 is the column count and must equal ncols
    colcount = struct.unpack("<H", data[record_start + 4:record_start + 6])[0]
    if colcount != ncols:
        return None
    # the columns must all decode as the four expected fields
    try:
        ends = offsets
        prev = record_start + (ends[0] - len(ADMIN_USERCODE))
        # reconstruct column boundaries: col0 starts at data_start
        starts = [data_start]
        for e in ends[:-1]:
            starts.append(record_start + e)
        vals = [data[s:record_start + e] for s, e in zip(starts, ends)]
    except Exception:
        return None
    if len(vals) < 4:
        return None
    if not vals[0].startswith(ADMIN_USERCODE):
        return None
    return offsets


def find_user_rows(data: bytes, usercode: bytes = ADMIN_USERCODE) -> list[UserRow]:
    """Find every row whose usercode column equals `usercode`."""
    rows: list[UserRow] = []
    start = 0
    while True:
        idx = data.find(usercode, start)
        if idx == -1:
            break
        start = idx + 1
        # the marker must be the start of a variable-data region: try each
        # candidate column count until the offsets look right
        for ncols in range(2, 17):
            offsets = _plausible(data, idx, ncols)
            if offsets is None:
                continue
            record_start = idx - (offsets[0] - len(usercode))
            starts = [idx]
            for e in offsets[:-1]:
                starts.append(record_start + e)
            cols = [data[s:record_start + e] for s, e in zip(starts, offsets)]
            # sanity: username/accesslevel/password are printable
            if all(all(32 <= b < 127 for b in c) for c in cols):
                rows.append(UserRow(idx // PAGE_SIZE, record_start, offsets, cols))
                break
    return rows


def read_users(mdf_path: str | Path) -> list[UserRow]:
    data = Path(mdf_path).read_bytes()
    return find_user_rows(data)


def get_password(mdf_path: str | Path) -> str | None:
    rows = read_users(mdf_path)
    for r in rows:
        if r.usercode == ADMIN_USERCODE.decode():
            return r.password
    return rows[0].password if rows else None


def set_password(mdf_path: str | Path, new_pin: str) -> None:
    """
    Replace the admin password in place and recompute the page checksum.

    The new PIN must be the same length as the current one so the variable
    column offsets stay valid (the shipped admin PINs are 5 characters).
    For a different length use dev/sql/reset_admin_pin.sql against an attached
    database instead.
    """
    path = Path(mdf_path)
    data = bytearray(path.read_bytes())
    rows = [r for r in find_user_rows(bytes(data)) if r.usercode == ADMIN_USERCODE.decode()]
    if not rows:
        raise SystemExit(f"{path}: admin row (usercode {ADMIN_USERCODE.decode()}) not found")
    row = rows[0]
    start, end = row.password_span
    old_len = end - start
    if len(new_pin) != old_len:
        raise SystemExit(
            f"{path}: new PIN is {len(new_pin)} chars but the stored PIN is "
            f"{old_len}; use dev/sql/reset_admin_pin.sql for a different length"
        )
    data[start:end] = new_pin.encode("latin-1")
    page_start = row.page_no * PAGE_SIZE
    page = bytes(data[page_start:page_start + PAGE_SIZE])
    ck = page_checksum(page)
    struct.pack_into("<I", data, page_start + CHECKSUM_OFFSET, ck)
    path.write_bytes(data)


def verify_page(mdf_path: str | Path) -> bool:
    """Re-read the file and confirm the admin page checksum is valid."""
    data = Path(mdf_path).read_bytes()
    rows = [r for r in find_user_rows(data) if r.usercode == ADMIN_USERCODE.decode()]
    if not rows:
        return False
    row = rows[0]
    page = data[row.page_no * PAGE_SIZE:(row.page_no + 1) * PAGE_SIZE]
    stored = struct.unpack("<I", page[CHECKSUM_OFFSET:CHECKSUM_OFFSET + 4])[0]
    return page_checksum(page) == stored


def _main() -> None:
    if len(sys.argv) < 2:
        print(__doc__)
        print("usage: mdf_users.py <file.mdf> [<file.mdf> ...]")
        raise SystemExit(2)
    for arg in sys.argv[1:]:
        print(f"== {arg}")
        try:
            rows = read_users(arg)
        except FileNotFoundError:
            print("   (not found)")
            continue
        if not rows:
            print("   no user rows found")
            continue
        for r in rows:
            print(f"   page {r.page_no}: usercode={r.usercode!r} "
                  f"username={r.username!r} accesslevel={r.accesslevel!r} "
                  f"password={r.password!r}")
        print(f"   page checksum valid: {verify_page(arg)}")


if __name__ == "__main__":
    _main()
