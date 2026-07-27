namespace MemoryScanner;

static class ScanCommands
{
    public static void SearchValue(string[] args)
    {
        byte[] data = SnapshotLoader.Load(args[1]);
        int targetValue = int.Parse(args[2]);
        var options = ScanOptions.ParseFlags(args, 3);
        options = ScanOptions.MergePositional(options, data.Length,
            args.Length > 3 && !args[3].StartsWith("--") ? int.Parse(args[3]) : null,
            args.Length > 4 && !args[4].StartsWith("--") ? ParseHex(args[4]) : null,
            args.Length > 5 && !args[5].StartsWith("--") ? ParseHex(args[5]) : null);

        var (rangeStart, rangeEnd) = options.GetRange(data.Length);
        int count = 0;

        for (int address = rangeStart; address <= rangeEnd; address++)
        {
            int value = SnapshotValueReader.ReadValue(data, address, options.Size);
            if (value != targetValue)
            {
                continue;
            }

            ScanOutput.WriteMatch(address, $"{value}", ref count, options.MaxResults);
        }

        ScanOutput.WriteTotal(count, options.MaxResults, "matches");
    }

    public static void CompareChanged(string[] args)
    {
        byte[] before = SnapshotLoader.Load(args[1]);
        byte[] after = SnapshotLoader.Load(args[2]);
        int newValue = int.Parse(args[3]);
        var options = ScanOptions.ParseFlags(args, 4);
        options = ScanOptions.MergePositional(options, before.Length,
            args.Length > 4 && !args[4].StartsWith("--") ? int.Parse(args[4]) : null,
            args.Length > 5 && !args[5].StartsWith("--") ? ParseHex(args[5]) : null,
            args.Length > 6 && !args[6].StartsWith("--") ? ParseHex(args[6]) : null,
            args.Length > 7 && !args[7].StartsWith("--") ? int.Parse(args[7]) : null);

        var (rangeStart, rangeEnd) = options.GetRange(before.Length);
        int count = 0;

        for (int address = rangeStart; address <= rangeEnd; address++)
        {
            int valueBefore = SnapshotValueReader.ReadValue(before, address, options.Size);
            int valueAfter = SnapshotValueReader.ReadValue(after, address, options.Size);

            if (valueBefore == valueAfter || valueAfter != newValue)
            {
                continue;
            }

            if (options.OldValue.HasValue && valueBefore != options.OldValue.Value)
            {
                continue;
            }

            ScanOutput.WriteMatch(address, $"{valueBefore} -> {valueAfter}", ref count, options.MaxResults);
        }

        ScanOutput.WriteTotal(count, options.MaxResults, "matches");
    }

    public static void CompareStable(string[] args)
    {
        byte[] before = SnapshotLoader.Load(args[1]);
        byte[] after = SnapshotLoader.Load(args[2]);
        int targetValue = int.Parse(args[3]);
        var options = ScanOptions.ParseFlags(args, 4);
        options = ScanOptions.MergePositional(options, before.Length,
            args.Length > 4 && !args[4].StartsWith("--") ? int.Parse(args[4]) : null);

        var (rangeStart, rangeEnd) = options.GetRange(before.Length);
        int count = 0;

        for (int address = rangeStart; address <= rangeEnd; address++)
        {
            int valueBefore = SnapshotValueReader.ReadValue(before, address, options.Size);
            int valueAfter = SnapshotValueReader.ReadValue(after, address, options.Size);

            if (valueBefore != targetValue || valueAfter != targetValue)
            {
                continue;
            }

            ScanOutput.WriteMatch(address, $"{targetValue} (stable)", ref count, options.MaxResults);
        }

        ScanOutput.WriteTotal(count, options.MaxResults, "matches");
    }

    public static void IntersectChanged(string[] args)
    {
        byte[] first = SnapshotLoader.Load(args[1]);
        byte[] middle = SnapshotLoader.Load(args[2]);
        byte[] last = SnapshotLoader.Load(args[3]);
        var options = ScanOptions.ParseFlags(args, 4);
        options = ScanOptions.MergePositional(options, first.Length,
            args.Length > 4 && !args[4].StartsWith("--") ? int.Parse(args[4]) : null,
            args.Length > 5 && !args[5].StartsWith("--") ? ParseHex(args[5]) : null,
            args.Length > 6 && !args[6].StartsWith("--") ? ParseHex(args[6]) : null,
            null,
            args.Length > 7 && !args[7].StartsWith("--") ? int.Parse(args[7]) : null);

        int maxValue = options.MaxValue ?? int.MaxValue;
        var (rangeStart, rangeEnd) = options.GetRange(first.Length);
        int count = 0;

        for (int address = rangeStart; address <= rangeEnd; address++)
        {
            int valueFirst = SnapshotValueReader.ReadValue(first, address, options.Size);
            int valueMiddle = SnapshotValueReader.ReadValue(middle, address, options.Size);
            int valueLast = SnapshotValueReader.ReadValue(last, address, options.Size);

            if (valueFirst != valueLast || valueFirst == valueMiddle)
            {
                continue;
            }

            if (valueFirst < 0 || valueFirst > maxValue || valueMiddle < 0 || valueMiddle > maxValue)
            {
                continue;
            }

            ScanOutput.WriteMatch(address, $"{valueFirst} -> {valueMiddle} -> {valueLast}", ref count, options.MaxResults);
        }

        ScanOutput.WriteTotal(count, options.MaxResults, "matches");
    }

    public static void CompareDelta(string[] args)
    {
        byte[] before = SnapshotLoader.Load(args[1]);
        byte[] after = SnapshotLoader.Load(args[2]);
        int delta = int.Parse(args[3]);
        var options = ScanOptions.ParseFlags(args, 4);
        options = ScanOptions.MergePositional(options, before.Length,
            args.Length > 4 && !args[4].StartsWith("--") ? int.Parse(args[4]) : null,
            args.Length > 5 && !args[5].StartsWith("--") ? ParseHex(args[5]) : null,
            args.Length > 6 && !args[6].StartsWith("--") ? ParseHex(args[6]) : null);

        var (rangeStart, rangeEnd) = options.GetRange(before.Length);
        int count = 0;

        for (int address = rangeStart; address <= rangeEnd; address++)
        {
            int valueBefore = SnapshotValueReader.ReadValue(before, address, options.Size);
            int valueAfter = SnapshotValueReader.ReadValue(after, address, options.Size);

            if (valueAfter - valueBefore != delta)
            {
                continue;
            }

            ScanOutput.WriteMatch(address, $"{valueBefore} -> {valueAfter} (delta {delta})", ref count, options.MaxResults);
        }

        ScanOutput.WriteTotal(count, options.MaxResults, "matches");
    }

    public static void ChainMatch(string[] args)
    {
        int flagIndex = Array.FindIndex(args, argument => argument.StartsWith("--"));
        if (flagIndex < 0)
        {
            flagIndex = args.Length;
        }

        string[] snapshotPaths = args[1..flagIndex];
        if (snapshotPaths.Length < 2)
        {
            throw new ArgumentException("Usage: chain-match <f1> <f2> ... [--values v1,v2,...] [--size 4] [--region full]");
        }

        var options = ScanOptions.ParseFlags(args, flagIndex);
        if (options.ChainValues == null || options.ChainValues.Length != snapshotPaths.Length)
        {
            throw new ArgumentException($"Provide --values with exactly {snapshotPaths.Length} comma-separated integers.");
        }

        byte[][] snapshots = SnapshotLoader.LoadMany(snapshotPaths);
        var (rangeStart, rangeEnd) = options.GetRange(snapshots[0].Length);
        int count = 0;

        for (int address = rangeStart; address <= rangeEnd; address++)
        {
            bool matches = true;
            for (int snapshotIndex = 0; snapshotIndex < snapshots.Length; snapshotIndex++)
            {
                int value = SnapshotValueReader.ReadValue(snapshots[snapshotIndex], address, options.Size);
                if (value != options.ChainValues[snapshotIndex])
                {
                    matches = false;
                    break;
                }
            }

            if (!matches)
            {
                continue;
            }

            string chain = string.Join(" -> ", options.ChainValues);
            ScanOutput.WriteMatch(address, chain, ref count, options.MaxResults);
        }

        ScanOutput.WriteTotal(count, options.MaxResults, "matches");
    }

    public static void CompareBytes(string[] args)
    {
        byte[] before = SnapshotLoader.Load(args[1]);
        byte[] after = SnapshotLoader.Load(args[2]);
        byte? target = args.Length > 3 && !args[3].StartsWith("--") ? byte.Parse(args[3]) : null;
        var options = ScanOptions.ParseFlags(args, 3);
        if (!args.Any(argument => argument == "--max-results"))
        {
            options.MaxResults = target.HasValue ? 200 : 500;
        }

        var (rangeStart, rangeEnd) = ScanRange.ResolveByteRange(options.Region, options.RangeStart, options.RangeEnd, before.Length);
        int count = 0;

        for (int address = rangeStart; address <= rangeEnd; address++)
        {
            if (before[address] == after[address])
            {
                continue;
            }

            if (target.HasValue)
            {
                if (after[address] != target.Value)
                {
                    continue;
                }

                ScanOutput.WriteMatch(address, $"{before[address]} -> {after[address]}", ref count, options.MaxResults);
                continue;
            }

            byte addedBits = (byte)(after[address] & ~before[address]);
            byte removedBits = (byte)(before[address] & ~after[address]);
            string message = $"0x{before[address]:X2} -> 0x{after[address]:X2}";
            if (addedBits > 0)
            {
                message += $" (+ flag: 0x{addedBits:X2})";
            }

            if (removedBits > 0)
            {
                message += $" (- flag: 0x{removedBits:X2})";
            }

            ScanOutput.WriteMatch(address, message, ref count, options.MaxResults);
        }

        ScanOutput.WriteTotal(count, options.MaxResults, "differences");
    }

    static int ParseHex(string text) =>
        Convert.ToInt32(text.Replace("0x", ""), 16);
}
