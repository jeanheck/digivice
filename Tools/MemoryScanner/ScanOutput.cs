namespace MemoryScanner;

static class ScanOutput
{
    public static void WriteMatch(int address, string message, ref int count, int maxResults)
    {
        if (count < maxResults)
        {
            Console.WriteLine($"0x{address:X8}: {message}");
        }

        count++;
    }

    public static void WriteTotal(int count, int maxResults, string label)
    {
        Console.WriteLine($"\nTotal {label}: {count}");
        if (count > maxResults)
        {
            Console.WriteLine($"(showing first {maxResults}; use --max-results to see more)");
        }
    }
}
