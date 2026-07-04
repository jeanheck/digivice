namespace MemoryScanner;

sealed class ScanOptions
{
    public int Size { get; set; } = 1;
    public string? Region { get; set; }
    public int? RangeStart { get; set; }
    public int? RangeEnd { get; set; }
    public int MaxResults { get; set; } = 200;
    public int? OldValue { get; set; }
    public int? MaxValue { get; set; }
    public int[]? ChainValues { get; set; }

    public static ScanOptions ParseFlags(string[] args, int flagStartIndex)
    {
        var options = new ScanOptions();

        for (int index = flagStartIndex; index < args.Length; index++)
        {
            string token = args[index];
            if (!token.StartsWith("--"))
            {
                continue;
            }

            string flag = token[2..].ToLowerInvariant();
            string? value = index + 1 < args.Length && !args[index + 1].StartsWith("--")
                ? args[++index]
                : null;

            switch (flag)
            {
                case "size":
                    options.Size = int.Parse(value ?? throw new ArgumentException("--size requires a value."));
                    break;
                case "region":
                    options.Region = value ?? throw new ArgumentException("--region requires a value.");
                    break;
                case "range":
                    if (value == null)
                    {
                        throw new ArgumentException("--range requires start and end hex values.");
                    }

                    string[] parts = value.Split(',');
                    if (parts.Length != 2)
                    {
                        throw new ArgumentException("--range format: --range 0xSTART,0xEND");
                    }

                    options.RangeStart = ParseHex(parts[0]);
                    options.RangeEnd = ParseHex(parts[1]);
                    break;
                case "max-results":
                    options.MaxResults = int.Parse(value ?? throw new ArgumentException("--max-results requires a value."));
                    break;
                case "old-val":
                    options.OldValue = int.Parse(value ?? throw new ArgumentException("--old-val requires a value."));
                    break;
                case "max-val":
                    options.MaxValue = int.Parse(value ?? throw new ArgumentException("--max-val requires a value."));
                    break;
                case "values":
                    options.ChainValues = (value ?? throw new ArgumentException("--values requires a comma-separated list."))
                        .Split(',')
                        .Select(int.Parse)
                        .ToArray();
                    break;
                default:
                    throw new ArgumentException($"Unknown flag '--{flag}'.");
            }
        }

        SnapshotValueReader.ValidateSize(options.Size);
        return options;
    }

    public static ScanOptions MergePositional(ScanOptions options, int dataLength, params int?[] positional)
    {
        if (positional.Length > 0 && positional[0].HasValue && options.Size == 1)
        {
            options.Size = positional[0]!.Value;
            SnapshotValueReader.ValidateSize(options.Size);
        }

        if (positional.Length > 1 && positional[1].HasValue && !options.RangeStart.HasValue)
        {
            options.RangeStart = positional[1]!.Value;
        }

        if (positional.Length > 2 && positional[2].HasValue && !options.RangeEnd.HasValue)
        {
            options.RangeEnd = positional[2]!.Value;
        }

        if (positional.Length > 3 && positional[3].HasValue && !options.OldValue.HasValue)
        {
            options.OldValue = positional[3]!.Value;
        }

        if (positional.Length > 4 && positional[4].HasValue && !options.MaxValue.HasValue)
        {
            options.MaxValue = positional[4]!.Value;
        }

        return options;
    }

    public (int Start, int End) GetRange(int dataLength) =>
        ScanRange.Resolve(Region, RangeStart, RangeEnd, dataLength, Size);

    static int ParseHex(string text) =>
        Convert.ToInt32(text.Replace("0x", ""), 16);
}
