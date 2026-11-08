#!/usr/bin/env bash
#
# decompile.sh - decompile all managed .NET assemblies of Madhushala Pro
# into ./decode using ILSpy's command-line decompiler (ilspycmd).
#
# Usage:
#   dev/scripts/decompile.sh                 # decompile everything found under Debug/
#   dev/scripts/decompile.sh <path> ...      # decompile specific files/dirs
#
# Output: one folder per assembly under ./decode/<name>.decompiled.cs
#
set -uo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
SRC_DIR="${REPO_ROOT}/Debug"
OUT_DIR="${OUT_DIR:-${REPO_ROOT}/decode}"
ILSPY="${ILSPY:-ilspycmd}"

command -v "${ILSPY}" >/dev/null 2>&1 || {
  echo "error: '${ILSPY}' not found. Install with:" >&2
  echo "  dotnet tool install -g ilspycmd" >&2
  exit 1
}

mkdir -p "${OUT_DIR}"

# Inputs: arguments, or default to the app binary + all DLLs/EXEs under Debug/.
if [ "$#" -gt 0 ]; then
  inputs=("$@")
else
  mapfile -t inputs < <(find "${SRC_DIR}" -maxdepth 1 -type f \
      \( -iname '*.dll' -o -iname '*.exe' \) \
      ! -iname '*.vshost.exe' | sort)
fi

for f in "${inputs[@]}"; do
  [ -f "${f}" ] || { echo "skip (not a file): ${f}"; continue; }
  name="$(basename "${f}")"
  case "${name}" in
    *.vshost.exe) echo "skip (shim): ${name}"; continue ;;
  esac
  dest="${OUT_DIR}/${name}"
  echo "==> ${name}"
  # Single-file decompilation into decode/<assembly>/<assembly>.decompiled.cs
  # (this reproduces the existing decode/ layout).
  "${ILSPY}" -o "${dest}" "${f}" \
      >/dev/null 2>&1 || echo "   (warn: ilspycmd reported an error for ${name})"
done

echo "done. output in ${OUT_DIR}"
