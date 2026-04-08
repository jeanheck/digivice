$ErrorActionPreference = "Stop"

Write-Host "Building Digivice Release (V1)" -ForegroundColor Cyan

# 1. Obter os caminhos absolutos
$RootPath = "c:\Projetos\digivice"
$BackendPath = Join-Path $RootPath "Backend"
$FrontendPath = Join-Path $RootPath "Frontend"
$OutTempPath = Join-Path $RootPath "out_temp"
$TauriBinariesPath = Join-Path $FrontendPath "src-tauri\binaries"

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

# 4. Copiar artefatos para a pasta de Binários do Tauri
Write-Host "`n[3/4] Preparing Sidecar Binaries..." -ForegroundColor Yellow
if (-not (Test-Path $TauriBinariesPath)) {
    New-Item -ItemType Directory -Force -Path $TauriBinariesPath | Out-Null
}

# Renomeia para o target triplet que o Tauri exige para Windows (x64)
$ExePath = Join-Path $OutTempPath "Backend.exe"
$TauriExeTarget = Join-Path $TauriBinariesPath "backend-x86_64-pc-windows-msvc.exe"
Copy-Item -Path $ExePath -Destination $TauriExeTarget -Force

# Copiar appsettings e outros essenciais para a mesma pasta de recursos
Copy-Item -Path (Join-Path $OutTempPath "appsettings.json") -Destination (Join-Path $TauriBinariesPath "appsettings.json") -Force

# Limpar o temp
Remove-Item -Recurse -Force $OutTempPath -ErrorAction SilentlyContinue

# 5. Build Tauri App
Write-Host "`n[4/4] Building Tauri App (Frontend + Sidecar)..." -ForegroundColor Yellow
Set-Location $FrontendPath
# Instalamos o cli se precisar
npm install
# Vamos chamar o script custom de tauri build (caso estejamos pelo CLI puro)
npx tauri build

# Garantir que os arquivos necessários para o backend rodar sem crash existam na pasta final do Tauri!
$ReleaseDir = Join-Path $FrontendPath "src-tauri\target\release"
if (Test-Path $ReleaseDir) {
    Write-Host "Copying Database and appsettings to Release folder..." -ForegroundColor Cyan
    Copy-Item -Path (Join-Path $BackendPath "Database") -Destination (Join-Path $ReleaseDir "Database") -Recurse -Force
    Copy-Item -Path (Join-Path $BackendPath "appsettings.json") -Destination (Join-Path $ReleaseDir "appsettings.json") -Force
}

Write-Host "`nBuild Completed! Check Frontend/src-tauri/target/release/" -ForegroundColor Green
Set-Location $RootPath
