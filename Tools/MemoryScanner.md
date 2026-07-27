# MemoryScanner

Documentação completa: [MemoryScanner/README.md](MemoryScanner/README.md)

```powershell
cd Tools/MemoryScanner
dotnet run -- snapshot Snapshots/estado.bin
dotnet run -- chain-match Snapshots/a.bin Snapshots/b.bin Snapshots/c.bin --values 0,200,400 --size 4 --region full
```
