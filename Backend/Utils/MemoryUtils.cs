namespace Backend.Utils
{
    public static class MemoryUtils
    {
        public static int ReadInt32OffsetSafely(string? hexOffset, int fallback)
        {
            if (string.IsNullOrEmpty(hexOffset)) return fallback;
            try
            {
                // Note: The json has integer strings for these sizes/strides instead of hex
                if (int.TryParse(hexOffset, out int num)) return num;
                return Convert.ToInt32(hexOffset, 16);
            }
            catch { return fallback; }
        }

        public static short ReadInt16FromBlock(byte[] block, string? hexOffset)
        {
            if (string.IsNullOrEmpty(hexOffset) || block.Length == 0) return 0;
            try
            {
                int offset = Convert.ToInt32(hexOffset, 16);
                if (offset + 1 >= block.Length) return 0;
                return BitConverter.ToInt16(block, offset);
            }
            catch { return 0; }
        }

        public static int ReadInt32FromBlock(byte[] block, string? hexOffset)
        {
            if (string.IsNullOrEmpty(hexOffset) || block.Length == 0) return 0;
            try
            {
                int offset = Convert.ToInt32(hexOffset, 16);
                if (offset + 3 >= block.Length) return 0;
                return BitConverter.ToInt32(block, offset);
            }
            catch { return 0; }
        }

        public static int ParseHex(string? hexStr)
        {
            if (string.IsNullOrEmpty(hexStr)) return 0;
            try { return Convert.ToInt32(hexStr, 16); } catch { return 0; }
        }
    }
}
