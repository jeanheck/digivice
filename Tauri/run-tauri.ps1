param(
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$TauriArgs
)

$ErrorActionPreference = "Stop"

$TauriPath = $PSScriptRoot
$RootPath = Split-Path -Parent $TauriPath
$FrontendPath = Join-Path $RootPath "Frontend"

$env:TAURI_APP_PATH = $TauriPath
$env:TAURI_FRONTEND_PATH = $FrontendPath

Set-Location $FrontendPath

if ($null -eq $TauriArgs -or $TauriArgs.Count -eq 0) {
    npx tauri
} else {
    npx tauri @TauriArgs
}
