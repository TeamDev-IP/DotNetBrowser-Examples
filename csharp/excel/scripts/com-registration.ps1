[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [ValidateSet('register', 'unregister', 'status')]
    [string]$Action = 'status'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$ProgId = 'ExcelComAddin.Connect'
$FriendlyName = 'Sales Lead Add-in'
$Description = 'Excel add-in for newsletter/SMS integration'
$RootClassGuid = '{A1B2C3D4-E5F6-7890-ABCD-EF1234567880}'

function Write-Info([string]$message) {
    Write-Host "[INFO] $message" -ForegroundColor Cyan
}

function Write-Ok([string]$message) {
    Write-Host "[ OK ] $message" -ForegroundColor Green
}

function Write-Warn([string]$message) {
    Write-Host "[WARN] $message" -ForegroundColor Yellow
}

function Resolve-RepoRoot {
    if ($PSScriptRoot) {
        return (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
    }

    return (Get-Location).Path
}

function Get-ExcelBitness {
    $checks = @(
        @{ Path = 'HKLM:\SOFTWARE\Microsoft\Office\ClickToRun\Configuration'; Name = 'Platform' },
        @{ Path = 'HKLM:\SOFTWARE\Microsoft\Office\16.0\Excel'; Name = 'Bitness' },
        @{ Path = 'HKLM:\SOFTWARE\WOW6432Node\Microsoft\Office\16.0\Excel'; Name = 'Bitness' }
    )

    foreach ($check in $checks) {
        try {
            if (Test-Path $check.Path) {
                $value = (Get-ItemProperty -Path $check.Path -ErrorAction Stop).($check.Name)
                if ($value) {
                    $text = $value.ToString().ToLowerInvariant()
                    if ($text -match '64') { return 'x64' }
                    if ($text -match '32|86|x86') { return 'x86' }
                }
            }
        }
        catch {
        }
    }

    if ([Environment]::Is64BitOperatingSystem) {
        Write-Warn 'Could not detect Excel bitness from registry. Falling back to x64.'
        return 'x64'
    }

    Write-Warn 'Could not detect Excel bitness from registry. Falling back to x86.'
    return 'x86'
}

function Resolve-RegAsmPath([string]$bitness) {
    if ($bitness -eq 'x64') {
        return 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe'
    }

    return 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe'
}

function Resolve-DllPath {
    $repoRoot = Resolve-RepoRoot
    $candidate = Join-Path $repoRoot 'src/ExcelComAddin/bin/Debug/ExcelComAddin.dll'
    if (-not (Test-Path $candidate)) {
        throw "Add-in DLL not found at '$candidate'. Build the project first."
    }

    return (Resolve-Path $candidate).Path
}

function Set-ExcelAddinRegistryKey {
    $addinsPath = "HKCU:\Software\Microsoft\Office\Excel\Addins\$ProgId"
    New-Item -Path $addinsPath -Force | Out-Null

    New-ItemProperty -Path $addinsPath -Name 'FriendlyName' -Value $FriendlyName -PropertyType String -Force | Out-Null
    New-ItemProperty -Path $addinsPath -Name 'Description' -Value $Description -PropertyType String -Force | Out-Null
    New-ItemProperty -Path $addinsPath -Name 'LoadBehavior' -Value 3 -PropertyType DWord -Force | Out-Null
    New-ItemProperty -Path $addinsPath -Name 'CommandLineSafe' -Value 0 -PropertyType DWord -Force | Out-Null

    Write-Ok "Excel add-in registry key ensured at $addinsPath"
}

function Remove-ExcelAddinRegistryKey {
    $addinsPath = "HKCU:\Software\Microsoft\Office\Excel\Addins\$ProgId"
    if (Test-Path $addinsPath) {
        Remove-Item -Path $addinsPath -Recurse -Force
        Write-Ok "Removed Excel add-in registry key at $addinsPath"
    }
    else {
        Write-Info "Excel add-in registry key not present: $addinsPath"
    }
}

function Invoke-RegAsm([string]$regasmPath, [string]$dllPath, [switch]$unregister) {
    if (-not (Test-Path $regasmPath)) {
        throw "RegAsm not found at '$regasmPath'. Install .NET Framework 4.x Developer Pack."
    }

    $arguments = @($dllPath)
    if ($unregister) {
        $arguments += '/u'
    }
    else {
        $arguments += '/codebase'
    }

    Write-Info "Running RegAsm: $regasmPath $($arguments -join ' ')"
    & $regasmPath @arguments

    if ($LASTEXITCODE -ne 0) {
        throw "RegAsm failed with exit code $LASTEXITCODE. Try running PowerShell as Administrator."
    }

    Write-Ok 'RegAsm completed successfully.'
}

function Show-Status {
    $excelAddinPath = "HKCU:\Software\Microsoft\Office\Excel\Addins\$ProgId"
    $progidPath = "Registry::HKEY_CLASSES_ROOT\$ProgId"
    $clsidPath = "Registry::HKEY_CLASSES_ROOT\CLSID\$RootClassGuid"

    Write-Host ''
    Write-Host '=== Excel COM Add-in Status ===' -ForegroundColor Magenta

    if (Test-Path $excelAddinPath) {
        $item = Get-ItemProperty -Path $excelAddinPath
        Write-Ok 'Excel Add-ins key exists (HKCU).'
        Write-Host "  FriendlyName : $($item.FriendlyName)"
        Write-Host "  Description  : $($item.Description)"
        Write-Host "  LoadBehavior : $($item.LoadBehavior)"
    }
    else {
        Write-Warn 'Excel Add-ins key missing (HKCU).'
    }

    if (Test-Path $progidPath) {
        Write-Ok "ProgId registered in HKCR: $ProgId"
    }
    else {
        Write-Warn "ProgId missing in HKCR: $ProgId"
    }

    if (Test-Path $clsidPath) {
        Write-Ok "CLSID registered in HKCR: $RootClassGuid"
    }
    else {
        Write-Warn "CLSID missing in HKCR: $RootClassGuid"
    }

    Write-Host ''
}

$bitness = Get-ExcelBitness
$regasmPath = Resolve-RegAsmPath -bitness $bitness
$dll = $null

if ($Action -in @('register', 'unregister')) {
    $dll = Resolve-DllPath
    Write-Info "Excel bitness: $bitness"
    Write-Info "DLL path: $dll"
}

switch ($Action) {
    'register' {
        Invoke-RegAsm -regasmPath $regasmPath -dllPath $dll
        Set-ExcelAddinRegistryKey
        Show-Status
    }

    'unregister' {
        Invoke-RegAsm -regasmPath $regasmPath -dllPath $dll -unregister
        Remove-ExcelAddinRegistryKey
        Show-Status
    }

    'status' {
        Show-Status
    }
}
