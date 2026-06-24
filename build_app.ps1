$ErrorActionPreference = "Stop"

Write-Host "Building Digivice Release (V1)" -ForegroundColor Cyan

# 1. Obter os caminhos absolutos
$RootPath = "c:\Projetos\digivice"
$BackendPath = Join-Path $RootPath "Backend"
$FrontendPath = Join-Path $RootPath "Frontend"
$OutTempPath = Join-Path $RootPath "out_temp"
$TauriBinariesPath = Join-Path $FrontendPath "src-tauri\binaries"
$CopySidecarRuntimeScript = Join-Path $FrontendPath "scripts\copy-sidecar-runtime.ps1"

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
}

# 2. Compilar o Backend
Write-Host "`n[1/4] Publishing Backend (Single-File Self-Contained)..." -ForegroundColor Yellow
Set-Location $BackendPath
if (Test-Path $OutTempPath) { Remove-Item -Recurse -Force $OutTempPath }
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o $OutTempPath

# Ajustar AppSettings no Output (Mantido True a pedido para Debugging)
Write-Host "`n[2/4] Keeping Debugging enabled in AppSettings for diagnostic..." -ForegroundColor Yellow
$ConfigPath = Join-Path $OutTempPath "appsettings.json"
# if (Test-Path $ConfigPath) {
#     $json = Get-Content $ConfigPath -Raw
#     $json = $json -replace '"Debugging":\s*true', '"Debugging": false'
#     Set-Content -Path $ConfigPath -Value $json
# }

if (-not (Test-Path $ConfigPath)) {
    throw "Publish output is missing appsettings.json"
}

$PublishedMemoryPath = Join-Path $OutTempPath "Memory"
if (-not (Test-Path $PublishedMemoryPath)) {
    throw "Publish output is missing Memory folder"
}

# 4. Copiar artefatos para a pasta de Binários do Tauri
Write-Host "`n[3/4] Preparing Sidecar Binaries..." -ForegroundColor Yellow
if (Test-Path $TauriBinariesPath) {
    Remove-Item -Recurse -Force $TauriBinariesPath
}

New-Item -ItemType Directory -Force -Path $TauriBinariesPath | Out-Null

# Renomeia para o target triplet que o Tauri exige para Windows (x64)
$ExePath = Join-Path $OutTempPath "Backend.exe"
$TauriExeTarget = Join-Path $TauriBinariesPath "backend-x86_64-pc-windows-msvc.exe"
Copy-Item -Path $ExePath -Destination $TauriExeTarget -Force

Copy-SidecarRuntimeAssets -SourceDirectory $OutTempPath -DestinationDirectory $TauriBinariesPath

# Limpar o temp
Remove-Item -Recurse -Force $OutTempPath -ErrorAction SilentlyContinue

# 5. Build Tauri App
Write-Host "`n[4/4] Building Tauri App (Frontend + Sidecar)..." -ForegroundColor Yellow
Set-Location $FrontendPath
# Instalamos o cli se precisar
npm install
# beforeBundleCommand no tauri.conf.json copia appsettings + Memory para target/release antes do bundle
npx tauri build

# Garantia extra após o build completo
if (Test-Path $CopySidecarRuntimeScript) {
    & $CopySidecarRuntimeScript
}

Write-Host "`nBuild Completed! Check Frontend/src-tauri/target/release/" -ForegroundColor Green
Set-Location $RootPath
