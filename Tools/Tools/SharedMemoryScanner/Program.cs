using System;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace SharedMemoryScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== DW3 PS1 Shared Memory Scanner ===");
            Process[] processes = Process.GetProcessesByName("duckstation-qt-x64-ReleaseLTCG");
            if (processes.Length == 0) processes = Process.GetProcessesByName("duckstation-nogui");
            if (processes.Length == 0) processes = Process.GetProcessesByName("duckstation-qt");
            if (processes.Length == 0) processes = Process.GetProcessesByName("ePSXe");
            
            if (processes.Length == 0)
            {
                Console.WriteLine("Erro: Nenhum emulador encontrado.");
                return;
            }

            Process process = processes[0];
            string mapName = $"duckstation_{process.Id}";
            Console.WriteLine($"Conectando à Shared Memory: {mapName}");

            try
            {
                using var memoryMappedFile = MemoryMappedFile.OpenExisting(mapName);
                using var accessor = memoryMappedFile.CreateViewAccessor();
                
                long capacity = accessor.Capacity;
                Console.WriteLine($"Tamanho da Memória Compartilhada do PS1: {capacity / 1024 / 1024} MB\n");

                List<long> possibleAddresses = new List<long>();
                int scanCount = 0;

                while (true)
                {
                    Console.WriteLine("\n---------------------------------------------------");
                    if (scanCount == 0)
                    {
                        Console.WriteLine("PASSO 1: Vá para qualquer mapa.");
                        Console.WriteLine("Digite a PRIMEIRA PALAVRA do nome desse mapa (ex: Central, Asuka, Seiryu):");
                    }
                    else
                    {
                        Console.WriteLine($"PASSO {scanCount + 1}: Vá para UM MAPA DIFERENTE.");
                        Console.WriteLine("Digite a PRIMEIRA PALAVRA do nome desse NOVO mapa:");
                        Console.WriteLine($"(Filtrando entre {possibleAddresses.Count} endereços suspeitos...)");
                    }

                    Console.Write("> ");
                    string targetString = Console.ReadLine() ?? "".Trim();
                    
                    if (string.IsNullOrEmpty(targetString) || targetString.Contains(" "))
                    {
                        Console.WriteLine("Erro: Digite apenas a primeira palavra sem espaços para não dar conflito na busca.");
                        continue;
                    }

                    byte[] targetBytes = new byte[targetString.Length];
                    for (int j = 0; j < targetString.Length; j++)
                    {
                        int charValue = (int)targetString[j];
                        if (charValue >= 65 && charValue <= 90) targetBytes[j] = (byte)(charValue - 0x33);
                        else if (charValue >= 97 && charValue <= 122) targetBytes[j] = (byte)(charValue - 0x39);
                        else targetBytes[j] = 0x00;
                    }

                    byte[] ram = new byte[capacity];
                    accessor.ReadArray(0, ram, 0, ram.Length);

                    if (scanCount == 0)
                    {
                        // First scan - search everywhere
                        for (long i = 0; i < ram.Length - targetBytes.Length; i++)
                        {
                            bool match = true;
                            for (int j = 0; j < targetBytes.Length; j++)
                            {
                                if (ram[i + j] != targetBytes[j])
                                {
                                    match = false;
                                    break;
                                }
                            }
                            if (match) possibleAddresses.Add(i);
                        }
                        Console.WriteLine($"\n[ SCAN INICIAL CONCLUÍDO - {possibleAddresses.Count} Endereços encontrados na RAM ]");
                    }
                    else
                    {
                        // Subsequent scans - filter existing addresses
                        List<long> survivingAddresses = new List<long>();
                        foreach (long addr in possibleAddresses)
                        {
                            bool match = true;
                            for (int j = 0; j < targetBytes.Length; j++)
                            {
                                if (addr + j >= ram.Length || ram[addr + j] != targetBytes[j])
                                {
                                    match = false;
                                    break;
                                }
                            }
                            if (match) survivingAddresses.Add(addr);
                        }
                        possibleAddresses = survivingAddresses;
                        Console.WriteLine($"\n[ FILTRO CONCLUÍDO - Restaram {possibleAddresses.Count} endereços ]");
                    }

                    if (possibleAddresses.Count > 0 && possibleAddresses.Count <= 5)
                    {
                        Console.WriteLine("\n✨ EUREKA! Encontramos o ponteiro do Buffer de UI! ✨");
                        foreach (var addr in possibleAddresses)
                        {
                            Console.WriteLine($"-> 0x{addr:X8} <- (Substitua na LocationAddress.cs)");
                        }
                        if (possibleAddresses.Count == 1) break;
                    }
                    else if (possibleAddresses.Count == 0)
                    {
                        Console.WriteLine("\n[X] Todos os endereços foram eliminados (A string sumiu). O Scanner vai ser resetado.");
                        scanCount = 0;
                        continue;
                    }

                    scanCount++;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao conectar no MMF: " + ex.Message);
            }
        }
    }
}
