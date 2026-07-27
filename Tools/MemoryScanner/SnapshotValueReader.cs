namespace MemoryScanner;

static class SnapshotValueReader
{
    public static int ReadValue(byte[] data, int offset, int size)
    {
        if (offset < 0 || offset + size > data.Length)
        {
            return 0;
        }

        return size switch
        {
            1 => data[offset],
            2 => BitConverter.ToInt16(data, offset),
            4 => BitConverter.ToInt32(data, offset),
            _ => throw new ArgumentException($"Unsupported size {size}. Use 1, 2, or 4.")
        };
    }

    public static void ValidateSize(int size)
    {
        if (size is not (1 or 2 or 4))
        {
            throw new ArgumentException($"Unsupported size {size}. Use 1, 2, or 4.");
        }
    }
}
