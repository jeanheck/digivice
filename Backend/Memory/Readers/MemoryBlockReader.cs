using Serilog;

namespace Backend.Memory.Readers
{
    public class MemoryBlockReader(byte[] memoryBlock)
    {
        private byte[] MemoryBlock { get; } = memoryBlock ?? throw new ArgumentNullException(nameof(memoryBlock));

        public short ReadInt16(int offset)
        {
            if (MemoryBlock.Length == 0 || offset + 1 >= MemoryBlock.Length)
            {
                Log.Warning(
                    "MemoryBlockReader.ReadInt16 out of bounds: offset={Offset}, length={Length}",
                    offset,
                    MemoryBlock.Length);
                return 0;
            }

            try
            {
                return BitConverter.ToInt16(MemoryBlock, offset);
            }
            catch (Exception ex)
            {
                Log.Warning(
                    "MemoryBlockReader.ReadInt16 failed at offset={Offset}: {Message}",
                    offset,
                    ex.Message);
                return 0;
            }
        }

        public int ReadInt32(int offset)
        {
            if (MemoryBlock.Length == 0 || offset + 3 >= MemoryBlock.Length)
            {
                Log.Warning(
                    "MemoryBlockReader.ReadInt32 out of bounds: offset={Offset}, length={Length}",
                    offset,
                    MemoryBlock.Length);
                return 0;
            }

            try
            {
                return BitConverter.ToInt32(MemoryBlock, offset);
            }
            catch (Exception ex)
            {
                Log.Warning(
                    "MemoryBlockReader.ReadInt32 failed at offset={Offset}: {Message}",
                    offset,
                    ex.Message);
                return 0;
            }
        }
    }
}
