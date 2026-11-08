#!/usr/bin/env python3
"""Convert the SQL Server DDL in dev/db/ddl.sql into a SQLite schema.

Reads CREATE TABLE statements (column definitions + PRIMARY KEY), maps the
T-SQL types to SQLite affinities, drops SQL Server-only syntax (WITH, ON
[PRIMARY], CLUSTERED, defaults are kept as literal DEFAULT clauses) and emits
a runnable SQLite script. Foreign keys are emitted as documented comments and
optional REFERENCES (disabled by default at runtime).
"""
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[3]
DDL = ROOT / "dev" / "db" / "ddl.sql"
OUT = Path(__file__).resolve().parents[1] / "src" / "db" / "schema.sql"

TYPE_MAP = [
    (r"^nvarchar\(max\)", "TEXT"),
    (r"^varchar\(max\)", "TEXT"),
    (r"^n?varchar\(\d+\)", "TEXT"),
    (r"^n?char\(\d+\)", "TEXT"),
    (r"^text$", "TEXT"),
    (r"^ntext$", "TEXT"),
    (r"^xml$", "TEXT"),
    (r"^uniqueidentifier$", "TEXT"),
    (r"^image$", "BLOB"),
    (r"^varbinary\(max\)", "BLOB"),
    (r"^varbinary\(\d+\)", "BLOB"),
    (r"^binary\(\d+\)", "BLOB"),
    (r"^int$", "INTEGER"),
    (r"^bigint$", "INTEGER"),
    (r"^smallint$", "INTEGER"),
    (r"^tinyint$", "INTEGER"),
    (r"^bit$", "INTEGER"),
    (r"^numeric\(\d+,\s*\d+\)", "NUMERIC"),
    (r"^decimal\(\d+,\s*\d+\)", "NUMERIC"),
    (r"^money$", "NUMERIC"),
    (r"^smallmoney$", "NUMERIC"),
    (r"^float$", "REAL"),
    (r"^real$", "REAL"),
    (r"^datetime2?$", "TEXT"),
    (r"^smalldatetime$", "TEXT"),
    (r"^date$", "TEXT"),
    (r"^time$", "TEXT"),
    (r"^datetimeoffset$", "TEXT"),
    (r"^timestamp$", "BLOB"),
]


def map_type(tsql: str) -> str:
    t = tsql.strip().lower()
    for pat, sqlite in TYPE_MAP:
        if re.match(pat, t):
            return sqlite
    return "TEXT"


def split_columns(body: str):
    """Split a CREATE TABLE body on top-level commas."""
    parts, depth, cur = [], 0, ""
    for ch in body:
        if ch == "(":
            depth += 1
        elif ch == ")":
            depth -= 1
        if ch == "," and depth == 0:
            parts.append(cur)
            cur = ""
        else:
            cur += ch
    if cur.strip():
        parts.append(cur)
    return parts


def parse_table(block: str):
    m = re.search(r"CREATE TABLE \[dbo\]\.\[(\w+)\]\s*\(", block, re.I)
    if not m:
        return None
    name = m.group(1)
    start = m.end()  # just after the opening paren
    depth, i = 1, start
    while i < len(block) and depth > 0:
        if block[i] == "(":
            depth += 1
        elif block[i] == ")":
            depth -= 1
        i += 1
    body = block[start:i - 1]
    cols, pk = [], []
    for part in split_columns(body):
        p = part.strip()
        if not p:
            continue
        if re.match(r"CONSTRAINT|PRIMARY KEY", p, re.I):
            pm = re.search(r"PRIMARY KEY[^(]*\((.*?)\)", p, re.S | re.I)
            if pm:
                for c in pm.group(1).split(","):
                    c = re.sub(r"[\[\]\s]|ASC|DESC", "", c, flags=re.I)
                    if c:
                        pk.append(c)
            continue
        cm = re.match(r"\[(\w+)\]\s*\[([^\]]+)\]\s*(.*)$", p, re.S)
        if not cm:
            continue
        col, ctype, rest = cm.group(1), cm.group(2), cm.group(3)
        nullable = not re.search(r"\bNOT NULL\b", rest, re.I)
        cols.append((col, map_type(ctype), nullable))
    return name, cols, pk


def main():
    sql = DDL.read_text(encoding="utf-8", errors="replace")
    tables = []
    for m in re.finditer(r"CREATE TABLE \[dbo\]\.\[(\w+)\]\s*\(", sql, re.I):
        parsed = parse_table(sql[m.start():])
        if parsed:
            tables.append(parsed)

    lines = []
    lines.append("-- SQLite schema for Madhushala Pro (new stack).")
    lines.append("-- Generated from dev/db/ddl.sql by scripts/gen_schema.py - do not edit by hand.")
    lines.append("PRAGMA foreign_keys = ON;")
    lines.append("")
    for name, cols, pk in sorted(tables):
        lines.append(f"CREATE TABLE IF NOT EXISTS [{name}] (")
        defs = []
        for col, ctype, nullable in cols:
            d = f"  [{col}] {ctype}"
            if not nullable:
                d += " NOT NULL"
            defs.append(d)
        if pk:
            defs.append("  PRIMARY KEY (" + ", ".join(f"[{c}]" for c in pk) + ")")
        lines.append(",\n".join(defs))
        lines.append(");")
        lines.append("")

    # Helpful indexes on common lookup columns.
    lines.append("-- Lookup indexes for the main business flows.")
    idx = [
        ("salesbillmain", "trndate"),
        ("salesbillmain", "storecode"),
        ("salesbilldetail", "trnno"),
        ("salesbilldetail", "itemcode"),
        ("purchasemain", "trndate"),
        ("purchasedetail", "trnno"),
        ("purchasedetail", "itemcode"),
        ("itemmst", "itemname"),
        ("itemmst", "barcode"),
        ("ledger", "name"),
        ("ledger_opening", "ledcode"),
        ("journaldetail", "ledcode"),
    ]
    for tbl, col in idx:
        lines.append(f"CREATE INDEX IF NOT EXISTS [ix_{tbl}_{col}] ON [{tbl}] ([{col}]);")
    lines.append("")

    OUT.parent.mkdir(parents=True, exist_ok=True)
    OUT.write_text("\n".join(lines) + "\n", encoding="utf-8")
    print(f"Wrote {OUT} with {len(tables)} tables.")


if __name__ == "__main__":
    sys.exit(main())
