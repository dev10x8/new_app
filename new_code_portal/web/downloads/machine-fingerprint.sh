#!/usr/bin/env bash
# machine-fingerprint.sh - collect the identifiers the VanijLiquor licence binds to.
#
# For Linux/macOS counter PCs (or for testing). Writes machine-fingerprint.json
# next to the script. Paste its contents into the portal under "Issue Licence".
set -u
dir="$(cd "$(dirname "$0")" && pwd)"

mac=""
for f in /sys/class/net/*/address; do
  [ -e "$f" ] || continue
  iface="$(basename "$(dirname "$f")")"
  [ "$iface" = "lo" ] && continue
  addr="$(cat "$f")"
  [ "$addr" = "00:00:00:00:00:00" ] && continue
  mac="$addr"; break
done
[ -z "$mac" ] && mac="$(ifconfig 2>/dev/null | awk '/ether/{print $2; exit}')"

cpu=""
if [ -r /proc/cpuinfo ]; then
  cpu="$(grep -m1 -E 'serial|model name' /proc/cpuinfo | sed 's/.*: //')"
fi

disk="$(lsblk -ndo SERIAL /dev/sda 2>/dev/null || true)"
hostname="$(hostname 2>/dev/null || echo '')"
os="$(uname -srm 2>/dev/null || echo '')"

cat > "$dir/machine-fingerprint.json" <<JSON
{
  "mac": "$mac",
  "macs": ["$mac"],
  "cpu": "$cpu",
  "disk": "$disk",
  "hostname": "$hostname",
  "os": "$os"
}
JSON

echo "Wrote $dir/machine-fingerprint.json:"
cat "$dir/machine-fingerprint.json"
