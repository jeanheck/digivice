namespace Backend.Utils
{
    public static class MemoryUtils
    {
        public static short ReadInt16FromBlock(byte[] block, int offset)
        {
            if (block.Length == 0 || offset + 1 >= block.Length) return 0;
            try
            {
                return BitConverter.ToInt16(block, offset);
            }
            catch { return 0; }
        }

        public static int ReadInt32FromBlock(byte[] block, int offset)
        {
            if (block.Length == 0 || offset + 3 >= block.Length) return 0;
            try
            {
                return BitConverter.ToInt32(block, offset);
            }
            catch { return 0; }
        }
    }
}
