using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Text.Json;

namespace MemoryScanner;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintHelp();
            return;
        }

        try
        {
            RunCommand(args);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error: {exception.Message}");
        }
    }

    static void RunCommand(string[] args)
    {
        var command = args[0].ToLowerInvariant();

        switch (command)
        {
            case "snapshot":
                RunSnapshot(args);
                break;
            case "compare":
                ScanCommands.CompareBytes(args);
                break;
            case "search-value":
                ScanCommands.SearchValue(args);
                break;
            case "compare-changed":
                ScanCommands.CompareChanged(args);
                break;
            case "compare-stable":
                ScanCommands.CompareStable(args);
                break;
            case "intersect-changed":
                ScanCommands.IntersectChanged(args);
                break;
            case "compare-delta":
                ScanCommands.CompareDelta(args);
                break;
            case "chain-match":
                ScanCommands.ChainMatch(args);
                break;
            case "dump":
                RunDump(args);
                break;
            case "fill":
                RunFill(args);
                break;
            case "analyze-pair":
                RunAnalyzePair(args);
                break;
            default:
                Console.WriteLine($"Unknown command '{command}'.");
                PrintHelp();
                break;
        }
    }

    static void PrintHelp()
    {
        Console.WriteLine("Commands:");
        Console.WriteLine("  snapshot <file.bin>");
        Console.WriteLine("  compare <f1> <f2> [targetByte] [--region quest|full|stats|inventory] [--range 0xSTART,0xEND] [--max-results N]");
        Console.WriteLine("  search-value <file> <value> [size] [rangeStart] [rangeEnd] [--size N] [--region NAME] [--range 0xS,0xE] [--max-results N]");
        Console.WriteLine("  compare-changed <f1> <f2> <newVal> [size] [rangeStart] [rangeEnd] [oldVal] [--old-val N] [--size N] [--region NAME]");
        Console.WriteLine("  compare-stable <f1> <f2> <value> [size] [--size N] [--region NAME]");
        Console.WriteLine("  compare-delta <f1> <f2> <delta> [size] [--size N] [--region NAME]");
        Console.WriteLine("  intersect-changed <f1> <f2> <f3> [size] [start] [end] [maxVal] [--max-val N] [--size N] [--region NAME]");
        Console.WriteLine("  chain-match <f1> <f2> ... --values v1,v2,... [--size 4] [--region full] [--max-results N]");
        Console.WriteLine("  dump <file.bin> <start-hex> <length>");
        Console.WriteLine("  analyze-pair <before.bin> <after.bin>");
        Console.WriteLine();
        Console.WriteLine("Regions: full (default typed), quest (default compare bytes), stats, inventory");
        Console.WriteLine("Value sizes: 1 = byte, 2 = Int16, 4 = Int32 (little-endian)");
    }

    static void RunSnapshot(string[] args)
    {
        var processes = Process.GetProcessesByName("duckstation-qt-x64-ReleaseLTCG");
        if (processes.Length == 0)
        {
            Console.WriteLine("DuckStation not found.");
            return;
        }

        string mapName = $"duckstation_{processes[0].Id}";
        using var memoryMappedFile = MemoryMappedFile.OpenExisting(mapName);
        using var accessor = memoryMappedFile.CreateViewAccessor();

        byte[] ram = new byte[SnapshotLoader.RamSize];
        accessor.ReadArray(0, ram, 0, SnapshotLoader.RamSize);
        File.WriteAllBytes(args[1], ram);
        Console.WriteLine($"Saved {args[1]}");
    }

    static void RunDump(string[] args)
    {
        byte[] data = SnapshotLoader.Load(args[1]);
        int start = Convert.ToInt32(args[2].Replace("0x", ""), 16);
        int length = int.Parse(args[3]);
        Console.Write($"0x{start:X8}: ");
        for (int index = 0; index < length && start + index < data.Length; index++)
        {
            Console.Write($"{data[start + index]:X2} ");
            if ((index + 1) % 16 == 0)
            {
                Console.Write($"\n0x{start + index + 1:X8}: ");
            }
        }

        Console.WriteLine();
    }

    static void RunFill(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Usage: fill <start-hex> <end-hex> <byteValue>");
            return;
        }

        int start = Convert.ToInt32(args[1].Replace("0x", ""), 16);
        int end = Convert.ToInt32(args[2].Replace("0x", ""), 16);
        byte value = byte.Parse(args[3]);

        var processes = Process.GetProcessesByName("duckstation-qt-x64-ReleaseLTCG");
        if (processes.Length == 0)
        {
            Console.WriteLine("DuckStation not found.");
            return;
        }

        string mapName = $"duckstation_{processes[0].Id}";
        using var memoryMappedFile = MemoryMappedFile.OpenExisting(mapName);
        using var accessor = memoryMappedFile.CreateViewAccessor();

        for (int address = start; address <= end; address++)
        {
            accessor.Write(address, value);
        }

        Console.WriteLine($"Filled addresses from 0x{start:X8} to 0x{end:X8} with value {value}.");
    }

    static void RunAnalyzePair(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: analyze-pair <before.bin> <after.bin>");
            return;
        }

        byte[] before = SnapshotLoader.Load(args[1]);
        byte[] after = SnapshotLoader.Load(args[2]);
        AnalyzePair(before, after);
    }

    static string? FindMainQuestDefinitionsPath()
    {
        string relative = Path.Combine("Backend", "Memory", "Definitions", "Quests", "MainQuestAddresses.json");
        var roots = new[]
        {
            Directory.GetCurrentDirectory(),
            AppContext.BaseDirectory,
            Path.GetDirectoryName(typeof(Program).Assembly.Location) ?? ""
        };

        foreach (var root in roots.Where(root => !string.IsNullOrEmpty(root)))
        {
            var directory = new DirectoryInfo(root);
            for (int depth = 0; depth < 8 && directory != null; depth++, directory = directory.Parent)
            {
                var candidate = Path.Combine(directory.FullName, relative);
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }
        }

        return null;
    }

    static void AnalyzePair(byte[] before, byte[] after)
    {
        const int mapIdAddress = 0x0004B3F8;
        const int encounterCacheStart = 0x0004B824;
        const int encounterCacheEnd = 0x0004BB00;
        const int playerBitsAddress = 0x00048DA0;

        Console.WriteLine("=== Map (should be equal if same area) ===");
        Console.WriteLine($"MapId @ 0x{mapIdAddress:X8}: 0x{before[mapIdAddress]:X2} -> 0x{after[mapIdAddress]:X2} (0227 = Divermon's Lake when 0x27)");

        Console.WriteLine("\n=== Main quest steps (MainQuestAddresses.json) ===");
        var questPath = FindMainQuestDefinitionsPath();

        if (questPath == null)
        {
            Console.WriteLine("Quest definitions not found (MainQuestAddresses.json).");
        }
        else
        {
            using var document = JsonDocument.Parse(File.ReadAllText(questPath));
            int changed = 0;
            foreach (var step in document.RootElement.GetProperty("Steps").EnumerateArray())
            {
                int number = step.GetProperty("Number").GetInt32();
                int address = Convert.ToInt32(step.GetProperty("Address").GetString()!.Replace("0x", ""), 16);
                var bitMasks = step.GetProperty("BitMasks")
                    .EnumerateArray()
                    .Select(maskElement => Convert.ToByte(maskElement.GetString()!.Replace("0x", ""), 16))
                    .ToList();
                bool was = IsStepDone(before[address], bitMasks);
                bool now = IsStepDone(after[address], bitMasks);
                if (was == now)
                {
                    continue;
                }

                changed++;
                string masksLabel = bitMasks.Count == 0
                    ? "raw"
                    : string.Join("+", bitMasks.Select(mask => $"0x{mask:X2}"));
                Console.WriteLine($"Step {number,2}: {(was ? "done" : "open")} -> {(now ? "done" : "open")}  @ 0x{address:X8} mask {masksLabel}  byte 0x{before[address]:X2}->0x{after[address]:X2}");
            }

            if (changed == 0)
            {
                Console.WriteLine("(no tracked main-quest bits changed)");
            }
        }

        Console.WriteLine("\n=== Encounter cache fingerprint (RAM pointers, session-specific) ===");
        int slots = 0;
        ushort? beforePointer = null;
        ushort? afterPointer = null;
        for (int offset = encounterCacheStart; offset < encounterCacheEnd; offset += 4)
        {
            ushort pointerBefore = BitConverter.ToUInt16(before, offset);
            ushort pointerAfter = BitConverter.ToUInt16(after, offset);
            byte stageBefore = before[offset + 2];
            byte stageAfter = after[offset + 2];
            if (pointerBefore == pointerAfter && stageBefore == stageAfter)
            {
                continue;
            }

            slots++;
            if (beforePointer == null)
            {
                beforePointer = pointerBefore;
                afterPointer = pointerAfter;
            }

            if (slots <= 3)
            {
                Console.WriteLine($"0x{offset:X8}: ptr 0x{pointerBefore:X4}->0x{pointerAfter:X4}  stage 0x{stageBefore:X2}->0x{stageAfter:X2}");
            }
        }

        if (slots > 3)
        {
            Console.WriteLine($"... {slots} slots changed (same pattern in each)");
        }

        if (beforePointer.HasValue)
        {
            Console.WriteLine($"Summary: first slot ptr 0x{beforePointer:X4} -> 0x{afterPointer:X4}  (early pool ~0x28AC, late pool ~0x8942 for Divermon's Lake)");
        }

        Console.WriteLine("\n=== Live RAM hook (Digivice backend) ===");
        bool step27 = (after[0x00048DBA] & 0x01) != 0;
        bool step28 = (after[0x0004B3DE] & 0x10) != 0;
        Console.WriteLine($"Step 27 complete: {step27}");
        Console.WriteLine($"Step 28 complete: {step28}");
        Console.WriteLine(step27
            ? "0227 enemies suggestion: 73 Crabmon, 103 Seadramon (76 Betamon likely out)"
            : "0227 enemies suggestion: 73 Crabmon, 76 Betamon");

        Console.WriteLine("\n=== Player bits @ 0x48DA0 (volatile, do not use alone) ===");
        Console.WriteLine($"before: {BitConverter.ToString(before, playerBitsAddress, 8)}");
        Console.WriteLine($"after:  {BitConverter.ToString(after, playerBitsAddress, 8)}");
    }

    static bool IsStepDone(byte rawValue, List<byte> bitMasks)
    {
        if (bitMasks.Count == 0)
        {
            return rawValue != 0;
        }

        foreach (byte bitMask in bitMasks)
        {
            if ((rawValue & bitMask) == 0)
            {
                return false;
            }
        }

        return true;
    }
}
