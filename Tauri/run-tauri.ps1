param(
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$TauriArgs
)

$ErrorActionPreference = "Stop"

$TauriPath = $PSScriptRoot
$RootPath = Split-Path -Parent $TauriPath
$AppPath = Join-Path $RootPath "App"

$env:TAURI_APP_PATH = $TauriPath
$env:TAURI_FRONTEND_PATH = $AppPath

Set-Location $AppPath

if ($null -eq $TauriArgs -or $TauriArgs.Count -eq 0) {
    npx tauri
} else {
    npx tauri @TauriArgs
}
