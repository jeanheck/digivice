using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace MemoryScanner
{
    class Program
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION64 lpBuffer, uint dwLength);

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION64
        {
            public ulong BaseAddress;
            public ulong AllocationBase;
            public uint AllocationProtect;
            public uint __alignment1;
            public ulong RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
            public uint __alignment2;
        }

        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const uint MEM_COMMIT = 0x1000;
        const uint PAGE_READWRITE = 0x04;

        static void Main(string[] args)
        {
            Console.WriteLine("=== DW3 ASCII String Scanner ===");
            Process[] processes = Process.GetProcessesByName("duckstation-qt-x64-ReleaseLTCG");
            if (processes.Length == 0) processes = Process.GetProcessesByName("duckstation-nogui");
            if (processes.Length == 0) processes = Process.GetProcessesByName("ePSXe");
            
            if (processes.Length == 0)
            {
                Console.WriteLine("Erro: Nenhum emulador (duckstation/ePSXe) encontrado rodando.");
                return;
            }

            Process process = processes[0];
            Console.WriteLine($"Vigilância iniciada no processo: {process.ProcessName} (PID: {process.Id})");

            IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);

            Console.Write("Digite o pedaço do nome do mapa exatamente como aparece (ex: Asuka City): ");
            string targetString = Console.ReadLine() ?? "";
            if (string.IsNullOrEmpty(targetString)) return;
            
            byte[] targetBytes = Encoding.ASCII.GetBytes(targetString);

            Console.WriteLine($"\nVarrendo blocos de memória em busca de '{targetString}'...");
            List<long> foundAddresses = new List<long>();

            long address = 0;
            long maxAddress = 0x7FFFFFFFFFFF; // Limite 64-bits usermode
            MEMORY_BASIC_INFORMATION64 memInfo = new MEMORY_BASIC_INFORMATION64();

            while (address < maxAddress)
            {
                int result = VirtualQueryEx(processHandle, (IntPtr)address, out memInfo, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION64)));
                if (result == 0) break;

                // Só pesquisa o que for bloco em uso e de Leitura+Escrita
                if (memInfo.State == MEM_COMMIT && memInfo.Protect == PAGE_READWRITE)
                {
                    byte[] buffer = new byte[memInfo.RegionSize];
                    int bytesRead = 0;
                    ReadProcessMemory((int)processHandle, (long)memInfo.BaseAddress, buffer, buffer.Length, ref bytesRead);

                    for (int i = 0; i <= bytesRead - targetBytes.Length; i++)
                    {
                        bool match = true;
                        for (int j = 0; j < targetBytes.Length; j++)
                        {
                            if (buffer[i + j] != targetBytes[j])
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match)
                        {
                            long foundAddress = (long)memInfo.BaseAddress + i;
                            foundAddresses.Add(foundAddress);
                            Console.WriteLine($"[ACHOU] Ponteiro em potencial -> 0x{foundAddress:X}");
                        }
                    }
                }
                address = (long)memInfo.BaseAddress + (long)memInfo.RegionSize;
            }

            Console.WriteLine($"\nBusca concluída. {foundAddresses.Count} endereços encontrados rolando esse texto.");
            Console.WriteLine("DICA: Mude de tela no jogo, anote os offsets, ou jogue o endereço na sua classe C# LocationAddress!");
        }
    }
}
