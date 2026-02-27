using System;
using System.IO;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Linq;

namespace MemoryScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Commands: snapshot <file.bin> | compare <file1.bin> <file2.bin> [value]");
                return;
            }

            var cmd = args[0].ToLower();

            if (cmd == "snapshot")
            {
                var procs = Process.GetProcessesByName("duckstation-qt-x64-ReleaseLTCG");
                if (procs.Length == 0)
                {
                    Console.WriteLine("DuckStation not found.");
                    return;
                }

                string mapName = $"duckstation_{procs[0].Id}";
                try
                {
                    using var mmf = MemoryMappedFile.OpenExisting(mapName);
                    using var accessor = mmf.CreateViewAccessor();

                    int ramSize = 2 * 1024 * 1024;
                    byte[] ram = new byte[ramSize];
                    accessor.ReadArray(0, ram, 0, ramSize);

                    File.WriteAllBytes(args[1], ram);
                    Console.WriteLine($"Saved {args[1]}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error mapping memory: {ex.Message}");
                }
            }
            else if (cmd == "compare")
            {
                byte[] d1 = File.ReadAllBytes(args[1]);
                byte[] d2 = File.ReadAllBytes(args[2]);
                byte? target = args.Length > 3 ? byte.Parse(args[3]) : null;

                int count = 0;
                for (int i = 0; i < d1.Length; i++)
                {
                    if (d1[i] != d2[i])
                    {
                        if (target.HasValue)
                        {
                            if (d2[i] != target.Value) continue;
                        }
                        else
                        {
                            // Filter for common key item flags: went from 0 to 1
                            if (d1[i] != 0 || d2[i] != 1) continue;
                            if (i < 0x00044000 || i > 0x00050000) continue;
                        }

                        if (count < 200)
                            Console.WriteLine($"0x{i:X8}: {d1[i]} -> {d2[i]}");
                        count++;
                    }
                }
                Console.WriteLine($"\nTotal differences: {count}");
            }
            else if (cmd == "fill" && args.Length == 4)
            {
                int start = Convert.ToInt32(args[1].Replace("0x", ""), 16);
                int end = Convert.ToInt32(args[2].Replace("0x", ""), 16);
                byte val = byte.Parse(args[3]);

                var procs = Process.GetProcessesByName("duckstation-qt-x64-ReleaseLTCG");
                if (procs.Length == 0)
                {
                    Console.WriteLine("DuckStation not found.");
                    return;
                }

                string mapName = $"duckstation_{procs[0].Id}";
                try
                {
                    using var mmf = MemoryMappedFile.OpenExisting(mapName);
                    using var accessor = mmf.CreateViewAccessor();

                    for (int i = start; i <= end; i++)
                    {
                        accessor.Write(i, val);
                    }
                    Console.WriteLine($"Filled addresses from 0x{start:X8} to 0x{end:X8} with value {val}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing memory: {ex.Message}");
                }
            }
        }
    }
}
