namespace MemoryScanner;

static class ScanRange
{
    public static (int Start, int End) Resolve(string? regionName, int? rangeStart, int? rangeEnd, int dataLength, int valueSize)
    {
        int endLimit = dataLength - valueSize;

        if (rangeStart.HasValue || rangeEnd.HasValue)
        {
            int start = rangeStart ?? 0;
            int end = rangeEnd ?? endLimit;
            return (start, Math.Min(end, endLimit));
        }

        if (!string.IsNullOrEmpty(regionName))
        {
            return regionName.ToLowerInvariant() switch
            {
                "full" => (0, endLimit),
                "quest" => (0x00048000, Math.Min(0x0004D000, endLimit)),
                "stats" => (0x000494000, Math.Min(0x000498000, endLimit)),
                "inventory" => (0x0004858F, Math.Min(0x000486FF, endLimit)),
                _ => throw new ArgumentException($"Unknown region '{regionName}'. Use: full, quest, stats, inventory.")
            };
        }

        return (0, endLimit);
    }

    public static (int Start, int End) ResolveByteRange(string? regionName, int? rangeStart, int? rangeEnd, int dataLength)
    {
        if (rangeStart.HasValue || rangeEnd.HasValue)
        {
            int start = rangeStart ?? 0;
            int end = rangeEnd ?? dataLength - 1;
            return (start, Math.Min(end, dataLength - 1));
        }

        if (!string.IsNullOrEmpty(regionName))
        {
            var (start, end) = Resolve(regionName, null, null, dataLength, 1);
            return (start, end);
        }

        return (0x00048000, Math.Min(0x0004D000, dataLength - 1));
    }
}
