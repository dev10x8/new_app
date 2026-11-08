#!/usr/bin/env bash
#
# admin_pin.sh - find or reset the Madhushala Pro admin login PIN.
#
# The login "PIN" is the plain-text `users.password` value (usercode A00001).
# This wrapper edits the SQL Server .MDF file directly (offline) and fixes the
# page checksum, so it works even when SQL Server is not running.
#
# Usage:
#   dev/scripts/admin_pin.sh find   [mdf ...]         # show current PIN(s)
#   dev/scripts/admin_pin.sh reset  <new-pin> [mdf ...]  # set PIN (same length)
#
# Defaults to both shipped databases when no .mdf is given:
#   Debug/Resources/barmanager.mdf  Debug/Resources/barmanager_demo.mdf
#
set -euo pipefail

HERE="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${HERE}/../.." && pwd)"
DEFAULT_MDFS=(
  "${REPO_ROOT}/Debug/Resources/barmanager.mdf"
  "${REPO_ROOT}/Debug/Resources/barmanager_demo.mdf"
)

usage() { sed -n '3,14p' "${BASH_SOURCE[0]}" | sed 's/^# \{0,1\}//'; exit 2; }

cmd="${1:-}"; shift || true

case "${cmd}" in
  find)
    mdfs=("$@"); [ "${#mdfs[@]}" -eq 0 ] && mdfs=("${DEFAULT_MDFS[@]}")
    python3 "${HERE}/mdf_users.py" "${mdfs[@]}"
    ;;
  reset)
    pin="${1:-}"; shift || true
    [ -z "${pin}" ] && usage
    mdfs=("$@"); [ "${#mdfs[@]}" -eq 0 ] && mdfs=("${DEFAULT_MDFS[@]}")
    for f in "${mdfs[@]}"; do
      if [ ! -f "${f}" ]; then echo "skip (not found): ${f}"; continue; fi
      cp -p "${f}" "${f}.bak.$(date +%Y%m%d%H%M%S)"
      python3 -c "
import sys
sys.path.insert(0, '${HERE}')
import mdf_users as m
m.set_password('${f}', '${pin}')
assert m.verify_page('${f}'), 'checksum verify failed'
print('${f}: PIN set to ${pin}, checksum ok')
"
    done
    ;;
  *)
    usage
    ;;
esac
