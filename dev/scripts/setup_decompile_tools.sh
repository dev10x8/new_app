#!/usr/bin/env bash
#
# setup_decompile_tools.sh - install the toolchain used to decompile the app.
#
# Installs:
#   * .NET SDK 8 (if missing)
#   * ilspycmd (global dotnet tool) - the ILSpy command-line decompiler
#   * git-lfs (the main EXE is stored in Git LFS)
#
# Usage: dev/scripts/setup_decompile_tools.sh
#
set -uo pipefail

echo "== .NET SDK =="
if command -v dotnet >/dev/null 2>&1; then
  dotnet --version
else
  echo "Installing .NET SDK 8 ..."
  curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
  bash /tmp/dotnet-install.sh --channel 8.0
  export PATH="$HOME/.dotnet:$PATH"
fi

echo "== ilspycmd =="
if command -v ilspycmd >/dev/null 2>&1; then
  ilspycmd --version | head -1
else
  echo "Installing ilspycmd ..."
  dotnet tool install -g ilspycmd
  export PATH="$HOME/.dotnet/tools:$PATH"
fi

echo "== git-lfs =="
if command -v git-lfs >/dev/null 2>&1; then
  git lfs version | head -1
else
  echo "Installing git-lfs ..."
  # Debian/Ubuntu
  curl -fsSL https://packagecloud.io/install/repositories/github/git-lfs/script.deb.sh | bash
  apt-get install -y git-lfs || echo "warn: install git-lfs manually"
  git lfs install
fi

echo
echo "Ensure these are on PATH: \$HOME/.dotnet and \$HOME/.dotnet/tools"
echo "Then run: dev/scripts/decompile.sh"
