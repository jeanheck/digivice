namespace Backend.Memory.Readers
{
    public class MemoryBlockReader(byte[] memoryBlock)
    {
        private byte[] MemoryBlock { get; } = memoryBlock ?? throw new ArgumentNullException(nameof(memoryBlock));

        public short ReadInt16(int offset)
        {
            if (MemoryBlock.Length == 0 || offset + 1 >= MemoryBlock.Length)
            {
                return 0;
            }

            try
            {
                return BitConverter.ToInt16(MemoryBlock, offset);
            }
            catch
            {
                return 0;
            }
        }

        public int ReadInt32(int offset)
        {
            if (MemoryBlock.Length == 0 || offset + 3 >= MemoryBlock.Length)
            {
                return 0;
            }

            try
            {
                return BitConverter.ToInt32(MemoryBlock, offset);
            }
            catch
            {
                return 0;
            }
        }
    }
}
