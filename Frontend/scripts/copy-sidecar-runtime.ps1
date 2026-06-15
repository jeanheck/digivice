$ErrorActionPreference = "Stop"

$FrontendPath = Split-Path -Parent $PSScriptRoot
$TauriPath = Join-Path $FrontendPath "src-tauri"
$SourcePath = Join-Path $TauriPath "binaries"
$ReleasePath = Join-Path $TauriPath "target\release"

function Copy-SidecarRuntimeAssets {
    param(
        [string]$SourceDirectory,
        [string]$DestinationDirectory
    )

    $appsettingsSource = Join-Path $SourceDirectory "appsettings.json"
    $memorySource = Join-Path $SourceDirectory "Memory"

    if (-not (Test-Path $appsettingsSource)) {
        throw "appsettings.json not found at $appsettingsSource"
    }

    if (-not (Test-Path $memorySource)) {
        throw "Memory folder not found at $memorySource"
    }

    New-Item -ItemType Directory -Force -Path $DestinationDirectory | Out-Null
    Copy-Item -Path $appsettingsSource -Destination (Join-Path $DestinationDirectory "appsettings.json") -Force
    Copy-Item -Path $memorySource -Destination (Join-Path $DestinationDirectory "Memory") -Recurse -Force

    Write-Host "Sidecar runtime assets copied to $DestinationDirectory" -ForegroundColor Cyan
}

if (-not (Test-Path $SourcePath)) {
    Write-Warning "Sidecar binaries folder not found: $SourcePath"
    exit 0
}

if (-not (Test-Path $ReleasePath)) {
    Write-Warning "Release folder not found: $ReleasePath"
    exit 0
}

Copy-SidecarRuntimeAssets -SourceDirectory $SourcePath -DestinationDirectory $ReleasePath

Get-ChildItem -Path $ReleasePath -Recurse -Filter "backend*.exe" -ErrorAction SilentlyContinue | ForEach-Object {
    $sidecarDirectory = $_.DirectoryName
    if ($sidecarDirectory -ne $ReleasePath) {
        Copy-SidecarRuntimeAssets -SourceDirectory $SourcePath -DestinationDirectory $sidecarDirectory
    }
}
