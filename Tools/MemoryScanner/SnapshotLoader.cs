namespace MemoryScanner;

static class SnapshotLoader
{
    public const int RamSize = 2 * 1024 * 1024;

    public static byte[] Load(string path)
    {
        byte[] data = File.ReadAllBytes(path);
        if (data.Length != RamSize)
        {
            throw new InvalidOperationException($"Snapshot must be {RamSize} bytes (2 MiB PS1 RAM). Got {data.Length} bytes in '{path}'.");
        }

        return data;
    }

    public static byte[][] LoadMany(IReadOnlyList<string> paths) =>
        paths.Select(Load).ToArray();
}
