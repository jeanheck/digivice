$ErrorActionPreference = "Stop"

# Windows PowerShell does not stop on native commands with a non-zero exit code.
function Invoke-Step([string]$Command) {
    Invoke-Expression $Command
    if ($LASTEXITCODE -ne 0) {
        throw "'$Command' failed with exit code $LASTEXITCODE."
    }
}

Write-Host "Building Digivice" -ForegroundColor Cyan

Push-Location (Join-Path $PSScriptRoot "App")
try {
    Invoke-Step "npm install"
    Invoke-Step "npm run test:run"
    Invoke-Step "npm run tauri:build"
    Write-Host "`nBuild completed: Tauri/target/release/" -ForegroundColor Green
} finally {
    Pop-Location
}
