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
                Console.WriteLine("Commands:");
                Console.WriteLine("  snapshot <file.bin>                                                       - save PS1 RAM snapshot");
                Console.WriteLine("  compare <file1.bin> <file2.bin> [value]                                   - find bytes that went 0->1");
                Console.WriteLine("  search-value <file.bin> <value> [size=1] [range-start] [range-end]        - find addresses holding a value");
                Console.WriteLine("  compare-changed <f1> <f2> <new-val> [size] [range-start] [range-end]      - addresses that changed to new-val");
                Console.WriteLine("  compare-stable <f1> <f2> <value> [size]                                   - addresses that stayed at value");
                Console.WriteLine("  intersect-changed <f1> <f2> <f3> [size] [start] [end] [max-val]           - f1==f3 AND f1!=f2; max-val filters small ints");
                Console.WriteLine("  dump <file.bin> <start-hex> <length>                                      - dump raw hex bytes at an address");
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
            else if (cmd == "search-value")
            {
                byte[] data = File.ReadAllBytes(args[1]);
                int targetVal = Convert.ToInt32(args[2]);
                int size = args.Length > 3 ? int.Parse(args[3]) : 1;
                int rangeStart = args.Length > 4 ? Convert.ToInt32(args[4].Replace("0x", ""), 16) : 0;
                int rangeEnd = args.Length > 5 ? Convert.ToInt32(args[5].Replace("0x", ""), 16) : data.Length - size;
                int count = 0;
                for (int i = rangeStart; i < rangeEnd; i++)
                {
                    int val = size == 2 ? BitConverter.ToInt16(data, i) : data[i];
                    if (val == targetVal)
                    {
                        if (count < 200)
                            Console.WriteLine($"0x{i:X8}: {val}");
                        count++;
                    }
                }
                Console.WriteLine($"\nTotal matches: {count}");
            }
            else if (cmd == "compare-changed")
            {
                byte[] d1 = File.ReadAllBytes(args[1]);
                byte[] d2 = File.ReadAllBytes(args[2]);
                int newVal = Convert.ToInt32(args[3]);
                int size = args.Length > 4 ? int.Parse(args[4]) : 1;
                int rangeStart = args.Length > 5 ? Convert.ToInt32(args[5].Replace("0x", ""), 16) : 0;
                int rangeEnd = args.Length > 6 ? Convert.ToInt32(args[6].Replace("0x", ""), 16) : d1.Length - size;
                int count = 0;
                for (int i = rangeStart; i < rangeEnd; i++)
                {
                    int v1 = size == 2 ? BitConverter.ToInt16(d1, i) : d1[i];
                    int v2 = size == 2 ? BitConverter.ToInt16(d2, i) : d2[i];
                    if (v1 != v2 && v2 == newVal)
                    {
                        if (count < 200)
                            Console.WriteLine($"0x{i:X8}: {v1} -> {v2}");
                        count++;
                    }
                }
                Console.WriteLine($"\nTotal matches: {count}");
            }
            else if (cmd == "compare-stable")
            {
                byte[] d1 = File.ReadAllBytes(args[1]);
                byte[] d2 = File.ReadAllBytes(args[2]);
                int targetVal = Convert.ToInt32(args[3]);
                int size = args.Length > 4 ? int.Parse(args[4]) : 1;
                int count = 0;
                for (int i = 0; i < d1.Length - size; i++)
                {
                    int v1 = size == 2 ? BitConverter.ToInt16(d1, i) : d1[i];
                    int v2 = size == 2 ? BitConverter.ToInt16(d2, i) : d2[i];
                    if (v1 == targetVal && v2 == targetVal)
                    {
                        if (count < 200)
                            Console.WriteLine($"0x{i:X8}: {targetVal} (stable)");
                        count++;
                    }
                }
                Console.WriteLine($"\nTotal matches: {count}");
            }
            else if (cmd == "intersect-changed")
            {
                // intersect-changed <f1> <f2> <f3> [size=1] [range-start] [range-end] [max-val]
                // Finds addresses where: f1 == f3 (returned to original) AND f1 != f2 (changed in middle)
                // If max-val is provided, only shows results where 0 <= v1,v2 <= max-val
                byte[] d1 = File.ReadAllBytes(args[1]);
                byte[] d2 = File.ReadAllBytes(args[2]);
                byte[] d3 = File.ReadAllBytes(args[3]);
                int size = args.Length > 4 ? int.Parse(args[4]) : 1;
                int rangeStart = args.Length > 5 ? Convert.ToInt32(args[5].Replace("0x", ""), 16) : 0;
                int rangeEnd = args.Length > 6 ? Convert.ToInt32(args[6].Replace("0x", ""), 16) : d1.Length - size;
                int maxVal = args.Length > 7 ? int.Parse(args[7]) : int.MaxValue;
                int count = 0;
                for (int i = rangeStart; i < rangeEnd; i++)
                {
                    int v1 = size == 2 ? BitConverter.ToInt16(d1, i) : d1[i];
                    int v2 = size == 2 ? BitConverter.ToInt16(d2, i) : d2[i];
                    int v3 = size == 2 ? BitConverter.ToInt16(d3, i) : d3[i];
                    if (v1 == v3 && v1 != v2 && v1 >= 0 && v1 <= maxVal && v2 >= 0 && v2 <= maxVal)
                    {
                        if (count < 300)
                            Console.WriteLine($"0x{i:X8}: {v1} -> {v2} -> {v3}");
                        count++;
                    }
                }
                Console.WriteLine($"\nTotal matches: {count}");
            }
            else if (cmd == "dump")
            {
                // dump <file.bin> <start-hex> <length>
                byte[] data = File.ReadAllBytes(args[1]);
                int start = Convert.ToInt32(args[2].Replace("0x", ""), 16);
                int len = int.Parse(args[3]);
                Console.Write($"0x{start:X8}: ");
                for (int i = 0; i < len && start + i < data.Length; i++)
                {
                    Console.Write($"{data[start + i]:X2} ");
                    if ((i + 1) % 16 == 0) Console.Write($"\n0x{start + i + 1:X8}: ");
                }
                Console.WriteLine();
            }
        }
    }
}
