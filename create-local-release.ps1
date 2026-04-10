# Este script simula exatamente os passos que o GitHub Actions realiza.
# Ideal para testar a compilação localmente antes de invocar uma Release oficial no Git.
# Uso: .\create-local-release.ps1 (a partir da raiz do projeto)

$ErrorActionPreference = "Stop"

# Caminhos absolutos (mesmo padrão do build_app.ps1 que funciona)
$RootPath       = "c:\Projetos\digivice"
$BackendPath    = Join-Path $RootPath "Backend"
$FrontendPath   = Join-Path $RootPath "Frontend"
$TauriBinsPath  = Join-Path $FrontendPath "src-tauri\binaries"
$TauriRelease   = Join-Path $FrontendPath "src-tauri\target\release"
$LocalRelease   = Join-Path $RootPath "LocalRelease"
$BackendDist    = Join-Path $LocalRelease "backend_dist"
$PacoteFinal    = Join-Path $LocalRelease "PacoteFinal"

Write-Host "`n[1/7] Limpando processos que podem travar arquivos..." -ForegroundColor Cyan
$processos = @("digivice", "Backend", "backend-x86_64-pc-windows-msvc", "build-script-build", "cargo", "rustc")
foreach ($proc in $processos) {
    Stop-Process -Name $proc -Force -ErrorAction SilentlyContinue
}
Write-Host "   Aguardando o Windows liberar os locks..." -ForegroundColor DarkGray
Start-Sleep -Seconds 3

Write-Host "`n[2/7] Limpando pasta de binários do Tauri..." -ForegroundColor Cyan
if (Test-Path $TauriBinsPath) { Remove-Item -Path $TauriBinsPath -Recurse -Force }

Write-Host "`n[3/7] Preparando workspace LocalRelease..." -ForegroundColor Cyan
if (Test-Path $LocalRelease) { Remove-Item $LocalRelease -Recurse -Force }
New-Item -ItemType Directory -Force -Path $BackendDist | Out-Null
New-Item -ItemType Directory -Force -Path $PacoteFinal | Out-Null

Write-Host "`n[4/7] Compilando o Backend (.NET)..." -ForegroundColor Cyan
Set-Location $BackendPath
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o $BackendDist
if ($LASTEXITCODE -ne 0) { Write-Host "ERRO: Falha ao compilar o Backend." -ForegroundColor Red; exit 1 }

Write-Host "`n[5/7] Injetando Sidecar no bundle do Tauri..." -ForegroundColor Cyan
New-Item -ItemType Directory -Force -Path $TauriBinsPath | Out-Null
Copy-Item -Path (Join-Path $BackendDist "Backend.exe") `
          -Destination (Join-Path $TauriBinsPath "backend-x86_64-pc-windows-msvc.exe") -Force
Copy-Item -Path (Join-Path $BackendDist "appsettings.json") `
          -Destination (Join-Path $TauriBinsPath "appsettings.json") -Force

Write-Host "`n[6/7] Compilando o Frontend (Tauri)... Isso pode demorar alguns minutos." -ForegroundColor Cyan
Set-Location $FrontendPath
npm install --silent
# Limpa o cache de build do Rust/Cargo para evitar locks de arquivo no Windows
# (no GitHub Actions isso não é necessário pois a máquina é zerada a cada run)
Write-Host "   Limpando cache de build do Rust (evita file lock no Windows)..." -ForegroundColor DarkGray
cargo clean --manifest-path src-tauri\Cargo.toml --release
npx tauri build
if ($LASTEXITCODE -ne 0) { Write-Host "ERRO: Falha ao compilar o Tauri. Abortando." -ForegroundColor Red; exit 1 }

Write-Host "`n[7/7] Empacotando a Release final..." -ForegroundColor Cyan
Set-Location $RootPath
Copy-Item -Path (Join-Path $TauriRelease "digivice.exe") `
          -Destination (Join-Path $PacoteFinal "digivice.exe") -Force
Copy-Item -Path (Join-Path $BackendPath "Database") `
          -Destination (Join-Path $PacoteFinal "Database") -Recurse -Force

$ZipPath = Join-Path $LocalRelease "Digivice-Windows-Portable.zip"
if (Test-Path $ZipPath) { Remove-Item $ZipPath -Force }
Compress-Archive -Path "$PacoteFinal\*" -DestinationPath $ZipPath

# Limpa intermediários, deixa só o ZIP
Remove-Item -Path $BackendDist -Recurse -Force
Remove-Item -Path $PacoteFinal -Recurse -Force

Write-Host "`nSUCESSO! Pacote criado em: LocalRelease\Digivice-Windows-Portable.zip" -ForegroundColor Green
