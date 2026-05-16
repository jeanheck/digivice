namespace Backend.Utils
{
    public class MemoryBlockReader(byte[] memoryBlock)
    {
        public short ReadInt16(int offset)
        {
            if (memoryBlock.Length == 0 || offset + 1 >= memoryBlock.Length)
            {
                return 0;
            }

            try
            {
                return BitConverter.ToInt16(memoryBlock, offset);
            }
            catch
            {
                return 0;
            }
        }

        public int ReadInt32(int offset)
        {
            if (memoryBlock.Length == 0 || offset + 3 >= memoryBlock.Length)
            {
                return 0;
            }

            try
            {
                return BitConverter.ToInt32(memoryBlock, offset);
            }
            catch
            {
                return 0;
            }
        }
    }
}
