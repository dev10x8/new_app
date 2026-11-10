# machine-fingerprint.ps1 - collect the identifiers the VanijLiquor licence binds to.
#
# Run on the counter PC (PowerShell). Writes machine-fingerprint.json next to the
# script. Paste that file's contents into the portal under "Issue Licence".
#
#   powershell -ExecutionPolicy Bypass -File machine-fingerprint.ps1

$ErrorActionPreference = 'SilentlyContinue'

# Primary physical NIC MAC (fall back to the first adapter with a MAC).
$macs = Get-NetAdapter -Physical | Where-Object { $_.MacAddress } | Select-Object -ExpandProperty MacAddress
if (-not $macs) {
  $macs = Get-CimInstance Win32_NetworkAdapterConfiguration |
    Where-Object { $_.MACAddress } | Select-Object -ExpandProperty MACAddress
}

$cpu = (Get-CimInstance Win32_Processor | Select-Object -First 1 -ExpandProperty ProcessorId)

# System disk serial (the disk holding the OS).
$disk = (Get-CimInstance Win32_DiskDrive |
  Where-Object { $_.Index -eq 0 } | Select-Object -First 1 -ExpandProperty SerialNumber)
if (-not $disk) {
  $disk = (Get-CimInstance Win32_PhysicalMedia | Select-Object -First 1 -ExpandProperty SerialNumber)
}

$fp = [ordered]@{
  mac      = ($macs | Select-Object -First 1)
  macs     = @($macs)
  cpu      = "$cpu".Trim()
  disk     = "$disk".Trim()
  hostname = $env:COMPUTERNAME
  os       = (Get-CimInstance Win32_OperatingSystem | Select-Object -ExpandProperty Caption)
}

$fp | ConvertTo-Json | Out-File -Encoding utf8 -FilePath (Join-Path $PSScriptRoot 'machine-fingerprint.json')
Write-Host 'Wrote machine-fingerprint.json:'
Get-Content (Join-Path $PSScriptRoot 'machine-fingerprint.json')
