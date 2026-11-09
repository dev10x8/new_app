#Requires -Version 5.1
<#
.SYNOPSIS
    Find or reset the Madhushala Pro admin login PIN on this PC.

.DESCRIPTION
    Madhushala Pro keeps every login PIN in the `users` table as plain text
    (column `users.password`). The administrator account is:

        usercode = A00001   username = Admin   accesslevel = Admin

    The login screen simply runs:

        select * from users where usercode='A00001' and password='<typed PIN>'

    and grants admin rights when the row's username is "Admin". So the "PIN"
    is exactly the `password` column value.

    This script finds the app's SQL Server database (barmanager, shipped in the
    Debug\Resources folder) and either:

        Find  - prints the current admin PIN (recovering it), or
        Reset - writes a new PIN.

    Two engines are used:

      Offline (default) - reads/writes the .mdf file directly, so it works even
                          when SQL Server is stopped or the DB is not attached.
                          The SQL Server page checksum is recomputed so the file
                          stays valid. A new PIN must be the same length as the
                          stored one (the shipped PINs are 5 characters).

      Online            - runs an UPDATE through SQL Server when the instance is
                          reachable. Allows a PIN of any length (max 10 chars)
                          and is the right choice when the app/database is in
                          use (the .mdf is locked).

.PARAMETER Action
    Find (default) or Reset.

.PARAMETER NewPin
    The new PIN to set when Action = Reset.

.PARAMETER UserCode
    The user whose PIN is found/reset. Defaults to the admin, A00001.

.PARAMETER Mdf
    One or more .mdf files to work on. If omitted, the script searches the usual
    locations (the script folder, Debug\Resources next to it, the app folder).

.PARAMETER AppFolder
    The app folder that contains the Debug folder, used when the .mdf is not
    found automatically.

.PARAMETER Server
    SQL Server instance for the online method, for example ".\SQLEXPRESS".
    If omitted, common instances are probed.

.PARAMETER Database
    Database name for the online method. Defaults to barmanager.

.PARAMETER SqlUser / SqlPassword
    Optional SQL Server login for the online method. If omitted, Windows
    authentication is tried first, then the credentials in the app's
    "Madhushala Pro.exe.config".

.PARAMETER Online
    Force the online method.

.PARAMETER Offline
    Force the offline method.

.PARAMETER NoBackup
    Do not write a .bak copy before an offline edit.

.EXAMPLE
    .\Reset-MadhushalaAdminPin.ps1
    Interactive: choose Find or Reset.

.EXAMPLE
    .\Reset-MadhushalaAdminPin.ps1 -Action Find
    Show the current admin PIN.

.EXAMPLE
    .\Reset-MadhushalaAdminPin.ps1 -Action Reset -NewPin 12345
    Set the admin PIN to 12345 (offline; same length as the stored PIN).

.EXAMPLE
    .\Reset-MadhushalaAdminPin.ps1 -Action Reset -NewPin 987654 -Online -Server .\SQLEXPRESS
    Set a 6-character PIN through SQL Server.

.NOTES
    Read/write access to the .mdf and to SQL Server is required. Run PowerShell
    as Administrator if the app is installed under Program Files.
#>

[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [ValidateSet('Find', 'Reset')]
    [string]$Action,

    [Parameter(Position = 1)]
    [string]$NewPin,

    [string]$UserCode = 'A00001',
    [string[]]$Mdf,
    [string]$AppFolder,
    [string]$Server,
    [string]$Database = 'barmanager',
    [string]$SqlUser,
    [string]$SqlPassword,
    [switch]$Online,
    [switch]$Offline,
    [switch]$NoBackup
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

# SQL Server storage constants.
$PAGE_SIZE = 8192
$CHECKSUM_OFFSET = 60

# --------------------------------------------------------------------------
# Console helpers
# --------------------------------------------------------------------------
function Write-Head([string]$Text) { Write-Host ''; Write-Host "== $Text" -ForegroundColor Cyan }
function Write-Ok([string]$Text)   { Write-Host "   [ok]   $Text" -ForegroundColor Green }
function Write-Warn2([string]$Text){ Write-Host "   [warn] $Text" -ForegroundColor Yellow }
function Write-Err2([string]$Text) { Write-Host "   [err]  $Text" -ForegroundColor Red }
function Write-Info([string]$Text) { Write-Host "   $Text" }

function Write-Banner {
    Write-Host ''
    Write-Host '  Madhushala Pro - Admin PIN finder / resetter' -ForegroundColor White
    Write-Host '  -------------------------------------------' -ForegroundColor DarkGray
}

# --------------------------------------------------------------------------
# Low-level .mdf access
# --------------------------------------------------------------------------
function Get-Latin1 { [System.Text.Encoding]::GetEncoding(28591) }

function Rotl32([uint64]$Value, [int]$Bits) {
    $v = [uint64]($Value -band 4294967295)
    $b = $Bits -band 31
    if ($b -eq 0) { return $v }
    $r = (($v -shl $b) -bor ($v -shr (32 - $b))) -band 4294967295
    return [uint64]$r
}

# SQL Server page checksum (classic XOR variant): 16 sectors x 128 dwords.
function Get-PageChecksum([byte[]]$Page) {
    if ($Page.Length -ne $PAGE_SIZE) { throw "page must be $PAGE_SIZE bytes" }
    $words = New-Object 'uint32[]' 2048
    for ($i = 0; $i -lt 2048; $i++) {
        $words[$i] = [System.BitConverter]::ToUInt32($Page, $i * 4)
    }
    [uint64]$acc = 0
    for ($i = 0; $i -lt 128; $i++) {
        if ($i -ne 15) { $acc = $acc -bxor [uint64]$words[$i] }   # index 15 holds the stored checksum
    }
    [uint64]$ck = Rotl32 $acc 15
    for ($s = 1; $s -lt 16; $s++) {
        [uint64]$a = 0
        $base = $s * 128
        for ($j = 0; $j -lt 128; $j++) { $a = $a -bxor [uint64]$words[$base + $j] }
        $ck = $ck -bxor (Rotl32 $a (15 - $s))
    }
    return [uint32]($ck -band 4294967295)
}

# Return the variable-column end-offsets if the bytes at $DataStart form a
# plausible record, else $null.
function Test-RecordOffsets([byte[]]$Data, [int]$DataStart, [int]$NCols, [int]$CodeLen) {
    $need = 2 * $NCols
    if ($DataStart - $need -lt 0) { return $null }

    $offsets = New-Object 'int[]' $NCols
    for ($i = 0; $i -lt $NCols; $i++) {
        $o = $DataStart - $need + $i * 2
        $offsets[$i] = [int]$Data[$o] + ([int]$Data[$o + 1] -shl 8)
    }
    for ($i = 0; $i -lt $NCols - 1; $i++) {
        if ($offsets[$i] -ge $offsets[$i + 1]) { return $null }
    }
    if ($offsets[0] -le 0) { return $null }

    $recordStart = $DataStart - ($offsets[0] - $CodeLen)
    if ($recordStart -lt 0) { return $null }

    # header byte 4..6 is the column count
    $colCount = [int]$Data[$recordStart + 4] + ([int]$Data[$recordStart + 5] -shl 8)
    if ($colCount -ne $NCols) { return $null }

    return ,$offsets
}

function Get-RangeString([byte[]]$Data, [int]$Start, [int]$End) {
    if ($End -le $Start) { return '' }
    return (Get-Latin1).GetString($Data, $Start, $End - $Start)
}

# Find every users-table row whose usercode equals $UserCode.
function Find-UserRows([byte[]]$Data, [string]$UserCode) {
    $markerLen = $UserCode.Length
    $text = (Get-Latin1).GetString($Data)
    $rows = New-Object System.Collections.Generic.List[object]
    $pos = 0
    while ($true) {
        $idx = $text.IndexOf($UserCode, $pos, [System.StringComparison]::Ordinal)
        if ($idx -lt 0) { break }
        $pos = $idx + 1

        for ($ncols = 2; $ncols -le 16; $ncols++) {
            $offsets = Test-RecordOffsets -Data $Data -DataStart $idx -NCols $ncols -CodeLen $markerLen
            if (-not $offsets) { continue }

            $recordStart = $idx - ($offsets[0] - $markerLen)
            $user = Get-RangeString $Data $idx ($recordStart + $offsets[0])
            $name = Get-RangeString $Data ($recordStart + $offsets[0]) ($recordStart + $offsets[1])
            $lvl  = Get-RangeString $Data ($recordStart + $offsets[1]) ($recordStart + $offsets[2])
            $pwdS = $recordStart + $offsets[2]
            $pwdE = $recordStart + $offsets[3]
            $pwd  = Get-RangeString $Data $pwdS $pwdE

            if ($user -ne $UserCode) { continue }

            $rows.Add([pscustomobject]@{
                PageNo      = [int][math]::Floor($idx / $PAGE_SIZE)
                RecordStart = $recordStart
                PwdStart    = $pwdS
                PwdEnd      = $pwdE
                UserCode    = $user
                UserName    = $name
                AccessLevel = $lvl
                Password    = $pwd
            })
            break
        }
    }
    return $rows
}

function Test-PageChecksum([byte[]]$Data, [int]$PageNo) {
    $start = $PageNo * $PAGE_SIZE
    if ($start + $PAGE_SIZE -gt $Data.Length) { return $false }
    $page = New-Object byte[] $PAGE_SIZE
    [System.Array]::Copy($Data, $start, $page, 0, $PAGE_SIZE)
    $stored = [System.BitConverter]::ToUInt32($page, $CHECKSUM_OFFSET)
    return ((Get-PageChecksum $page) -eq $stored)
}

function Read-MdfFile([string]$Path) {
    try {
        return ,([System.IO.File]::ReadAllBytes($Path))
    } catch [System.IO.IOException] {
        throw "Cannot read '$Path' ($($_.Exception.Message)). Close Madhushala Pro (and SQL Server) and try again, or use -Online."
    }
}

# --------------------------------------------------------------------------
# Offline find / reset
# --------------------------------------------------------------------------
function Find-AdminPinOffline([string]$Path, [string]$UserCode) {
    $data = Read-MdfFile $Path
    $rows = @(Find-UserRows -Data $data -UserCode $UserCode)
    return $rows
}

function Set-AdminPinOffline([string]$Path, [string]$UserCode, [string]$Pin, [switch]$SkipBackup) {
    $data = Read-MdfFile $Path
    $rows = @(Find-UserRows -Data $data -UserCode $UserCode)
    if ($rows.Count -eq 0) { throw "no row for usercode '$UserCode' in '$Path'" }

    $row = $rows[0]
    $curLen = $row.PwdEnd - $row.PwdStart
    if ($Pin.Length -ne $curLen) {
        throw ("new PIN is $($Pin.Length) character(s) but the stored PIN is $curLen; " +
               "use -Online (SQL Server) to change the length, or pick a $curLen-character PIN")
    }

    if (-not $SkipBackup) {
        $stamp = Get-Date -Format 'yyyyMMddHHmmss'
        $bak = "$Path.bak.$stamp"
        Copy-Item -LiteralPath $Path -Destination $bak -Force
        Write-Info "backup: $bak"
    }

    $newBytes = (Get-Latin1).GetBytes($Pin)
    for ($k = 0; $k -lt $newBytes.Length; $k++) {
        $data[$row.PwdStart + $k] = $newBytes[$k]
    }

    # recompute and store the page checksum
    $pageStart = $row.PageNo * $PAGE_SIZE
    $page = New-Object byte[] $PAGE_SIZE
    [System.Array]::Copy($data, $pageStart, $page, 0, $PAGE_SIZE)
    $ck = Get-PageChecksum $page
    $ckBytes = [System.BitConverter]::GetBytes([uint32]$ck)
    [System.Array]::Copy($ckBytes, 0, $data, $pageStart + $CHECKSUM_OFFSET, 4)

    [System.IO.File]::WriteAllBytes($Path, $data)
    return $row
}

function Verify-AdminPinOffline([string]$Path, [string]$UserCode, [string]$Expected) {
    $data = Read-MdfFile $Path
    $rows = @(Find-UserRows -Data $data -UserCode $UserCode)
    if ($rows.Count -eq 0) { return $false }
    $row = $rows[0]
    return ($row.Password -eq $Expected -and (Test-PageChecksum $data $row.PageNo))
}

# --------------------------------------------------------------------------
# Database file discovery
# --------------------------------------------------------------------------
function Find-MdfFiles([string[]]$Explicit, [string]$AppFolder) {
    if ($Explicit) { return @($Explicit) }

    $dirs = New-Object System.Collections.Generic.List[string]
    if ($AppFolder) {
        $dirs.Add((Join-Path $AppFolder 'Resources'))
        $dirs.Add((Join-Path $AppFolder 'Debug\Resources'))
        $dirs.Add($AppFolder)
    }
    if ($PSScriptRoot) {
        $dirs.Add($PSScriptRoot)
        $dirs.Add((Join-Path $PSScriptRoot 'Resources'))
        $parent = Split-Path $PSScriptRoot -Parent
        if ($parent) {
            $dirs.Add((Join-Path $parent 'Resources'))
            $grand = Split-Path $parent -Parent
            if ($grand) {
                $dirs.Add((Join-Path $grand 'Debug\Resources'))
                $dirs.Add((Join-Path $grand 'Debug'))
            }
        }
    }
    $dirs.Add((Get-Location).Path)
    if ($env:ProgramFiles) {
        $dirs.Add((Join-Path $env:ProgramFiles 'Madhushala Pro\Debug\Resources'))
        $dirs.Add((Join-Path $env:ProgramFiles 'Madhushala Pro\Resources'))
    }
    if (${env:ProgramFiles(x86)}) {
        $dirs.Add((Join-Path ${env:ProgramFiles(x86)} 'Madhushala Pro\Debug\Resources'))
    }
    $dirs.Add('C:\Madhushala Pro\Debug\Resources')
    $dirs.Add('C:\Madhushala\Debug\Resources')

    $found = New-Object System.Collections.Generic.List[string]
    foreach ($d in ($dirs | Select-Object -Unique)) {
        if (-not (Test-Path -LiteralPath $d)) { continue }
        foreach ($f in (Get-ChildItem -LiteralPath $d -Filter '*.mdf' -File -ErrorAction SilentlyContinue)) {
            if (-not $found.Contains($f.FullName)) { $found.Add($f.FullName) }
        }
    }
    return $found
}

# --------------------------------------------------------------------------
# Online (SQL Server) find / reset
# --------------------------------------------------------------------------
function Get-ConfigCredentials([string]$AppFolder) {
    $candidates = @()
    if ($AppFolder) { $candidates += (Join-Path $AppFolder 'Debug\Madhushala Pro.exe.config') }
    if ($PSScriptRoot) {
        $candidates += (Join-Path $PSScriptRoot '..\..\Debug\Madhushala Pro.exe.config')
    }
    foreach ($c in $candidates) {
        if (-not (Test-Path -LiteralPath $c)) { continue }
        try {
            [xml]$xml = Get-Content -LiteralPath $c -Raw
            $user = ($xml.configuration.appSettings.add | Where-Object { $_.key -eq 'User' }).value
            $pass = ($xml.configuration.appSettings.add | Where-Object { $_.key -eq 'Password' }).value
            if ($user -and $pass) { return [pscustomobject]@{ User = $user; Password = $pass } }
        } catch { }
    }
    return $null
}

function Get-SqlServers([string]$Explicit) {
    $list = New-Object System.Collections.Generic.List[string]
    if ($Explicit) { $list.Add($Explicit) }
    foreach ($s in @('(local)', '.\SQLEXPRESS', 'localhost', '.\MADHUSHALA')) { $list.Add($s) }
    try {
        $key = Get-Item 'HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL' -ErrorAction Stop
        foreach ($name in $key.Property) {
            $inst = (Get-ItemProperty -Path $key.PSPath).$name
            $list.Add(".\$inst")
            $list.Add("(local)\$inst")
        }
    } catch { }
    return ($list | Select-Object -Unique)
}

function New-SqlConnection([string]$ServerName, [string]$Db, [string]$User, [string]$Pass) {
    if ($User) {
        $cs = "Server=$ServerName;Database=$Db;User ID=$User;Password=$Pass;Connect Timeout=4;TrustServerCertificate=True"
    } else {
        $cs = "Server=$ServerName;Database=$Db;Integrated Security=SSPI;Connect Timeout=4;TrustServerCertificate=True"
    }
    return (New-Object System.Data.SqlClient.SqlConnection $cs)
}

function Try-Online([string]$Action, [string]$ServerName, [string]$Db, [string]$UserCode,
                    [string]$NewPin, [string]$SqlUser, [string]$SqlPassword, [string]$AppFolder) {
    $creds = @()
    $creds += [pscustomobject]@{ User = $SqlUser; Password = $SqlPassword }
    $creds += [pscustomobject]@{ User = $null; Password = $null }        # Windows auth
    $cfg = Get-ConfigCredentials $AppFolder
    if ($cfg) { $creds += [pscustomobject]@{ User = $cfg.User; Password = $cfg.Password } }

    foreach ($srv in (Get-SqlServers $ServerName)) {
        foreach ($cred in $creds) {
            $conn = $null
            try {
                $conn = New-SqlConnection $srv $Db $cred.User $cred.Password
                $conn.Open()
                $cmd = $conn.CreateCommand()
                if ($Action -eq 'Find') {
                    $cmd.CommandText = "SELECT usercode, username, accesslevel, password FROM users WHERE usercode=@u"
                } else {
                    $cmd.CommandText = "UPDATE users SET password=@p WHERE usercode=@u"
                    [void]$cmd.Parameters.AddWithValue('@p', $NewPin)
                }
                [void]$cmd.Parameters.AddWithValue('@u', $UserCode)
                if ($Action -eq 'Find') {
                    $reader = $cmd.ExecuteReader()
                    $rows = New-Object System.Collections.Generic.List[object]
                    while ($reader.Read()) {
                        $rows.Add([pscustomobject]@{
                            Server      = $srv
                            UserCode    = $reader['usercode']
                            UserName    = $reader['username']
                            AccessLevel = $reader['accesslevel']
                            Password    = $reader['password']
                        })
                    }
                    $reader.Close()
                    if ($rows.Count -gt 0) { return [pscustomobject]@{ Server = $srv; Rows = $rows } }
                } else {
                    $n = $cmd.ExecuteNonQuery()
                    if ($n -gt 0) { return [pscustomobject]@{ Server = $srv; Updated = $n } }
                }
            } catch {
                # try the next server / credential
            } finally {
                if ($conn) { $conn.Dispose() }
            }
        }
    }
    return $null
}

# --------------------------------------------------------------------------
# Reporting
# --------------------------------------------------------------------------
function Show-Found([string]$Path, $Rows) {
    foreach ($r in $Rows) {
        Write-Host ("   {0}  (page {1})" -f $Path, $r.PageNo)
        Write-Host ("      User code    : {0}" -f $r.UserCode)
        Write-Host ("      User name    : {0}" -f $r.UserName)
        Write-Host ("      Access level : {0}" -f $r.AccessLevel)
        Write-Host ("      PIN          : {0}" -f $r.Password) -ForegroundColor Green
    }
}

function Show-LoginHelp([string]$UserCode, [string]$Pin, [string]$AccessLevel) {
    Write-Head 'How to log in'
    Write-Info '1. Start "Madhushala Pro.exe" (or the Madhushala Pro shortcut).'
    Write-Info '2. On the sign-in screen, enter:'
    Write-Host ("       User ID  : {0}" -f $UserCode) -ForegroundColor White
    Write-Host ("       Password : {0}" -f $Pin) -ForegroundColor White
    Write-Info '3. Click the login / OK button.'
    Write-Host ("   Access level: {0}" -f $AccessLevel) -ForegroundColor Cyan
    Write-Host ''
    Write-Info 'The Password field is the login PIN. It is stored as plain text in the'
    Write-Info 'users table (users.password), so the value above is exactly what to type.'
}

# --------------------------------------------------------------------------
# Main
# --------------------------------------------------------------------------
Write-Banner

# Interactive action if none was given.
if (-not $Action) {
    $choice = Read-Host 'Do you want to (F)ind the current PIN or (R)eset it? [F/r]'
    $Action = if ($choice -match '^[Rr]') { 'Reset' } else { 'Find' }
}

if ($Action -eq 'Reset' -and -not $NewPin) {
    $NewPin = Read-Host 'Enter the new admin PIN'
}
if ($Action -eq 'Reset' -and [string]::IsNullOrEmpty($NewPin)) {
    Write-Err2 'A new PIN is required.'
    exit 2
}

$mdfFiles = @(Find-MdfFiles -Explicit $Mdf -AppFolder $AppFolder)

# ---- online path ---------------------------------------------------------
if ($Online) {
    Write-Head "Online ($Action) via SQL Server"
    if ($Action -eq 'Find') {
        $res = Try-Online -Action 'Find' -ServerName $Server -Db $Database -UserCode $UserCode -AppFolder $AppFolder
        if ($res) {
            foreach ($r in $res.Rows) {
                Write-Ok ("found on {0}" -f $r.Server)
                Show-Found $Database $r
            }
            $first = $res.Rows[0]
            Show-LoginHelp $first.UserCode $first.Password $first.AccessLevel
            exit 0
        }
        Write-Warn2 'SQL Server not reachable; falling back to the offline method.'
    } else {
        $res = Try-Online -Action 'Reset' -ServerName $Server -Db $Database -UserCode $UserCode -NewPin $NewPin `
                          -SqlUser $SqlUser -SqlPassword $SqlPassword -AppFolder $AppFolder
        if ($res) {
            Write-Ok ("PIN updated on {0} ({1} row)" -f $res.Server, $res.Updated)
            Show-LoginHelp $UserCode $NewPin 'Admin'
            exit 0
        }
        Write-Warn2 'SQL Server not reachable; falling back to the offline method.'
    }
}

# ---- offline path --------------------------------------------------------
if (-not $mdfFiles -or $mdfFiles.Count -eq 0) {
    Write-Err2 'No database (.mdf) file found.'
    Write-Info 'Pass one explicitly, for example:'
    Write-Info '  .\Reset-MadhushalaAdminPin.ps1 -Action Find -Mdf "C:\Madhushala Pro\Debug\Resources\barmanager.mdf"'
    exit 3
}

Write-Head "Database file(s)"
foreach ($f in $mdfFiles) { Write-Info $f }

if ($Action -eq 'Find') {
    Write-Head 'Current admin PIN'
    $found = $null
    foreach ($f in $mdfFiles) {
        if (-not (Test-Path -LiteralPath $f)) { Write-Warn2 "not found: $f"; continue }
        try {
            $rows = @(Find-AdminPinOffline -Path $f -UserCode $UserCode)
        } catch {
            Write-Err2 $_.Exception.Message
            continue
        }
        if ($rows.Count -eq 0) { Write-Warn2 "no '$UserCode' row in $f"; continue }
        if (-not $found) { $found = $rows[0] }
        Show-Found $f $rows
    }
    if ($found) {
        Show-LoginHelp $found.UserCode $found.Password $found.AccessLevel
    } else {
        Write-Err2 "Admin account '$UserCode' was not found."
        exit 4
    }
    exit 0
}

# Action = Reset (offline)
Write-Head "Resetting admin PIN (offline)"
$done = $false
foreach ($f in $mdfFiles) {
    if (-not (Test-Path -LiteralPath $f)) { Write-Warn2 "not found: $f"; continue }
    try {
        $row = Set-AdminPinOffline -Path $f -UserCode $UserCode -Pin $NewPin -SkipBackup:$NoBackup
    } catch {
        Write-Err2 "$f : $($_.Exception.Message)"
        continue
    }
    if (Verify-AdminPinOffline -Path $f -UserCode $UserCode -Expected $NewPin) {
        Write-Ok ("{0}: PIN set to '{1}', page checksum ok" -f $f, $NewPin)
        $done = $true
    } else {
        Write-Err2 "$f : verification failed (checksum or value mismatch)"
    }
}

if (-not $done) {
    Write-Err2 'No database was updated. If the app or SQL Server is running, close it and retry, or use -Online.'
    exit 5
}

Show-LoginHelp $UserCode $NewPin 'Admin'
exit 0
