using System.IO.MemoryMappedFiles;

namespace Backend.Infrastructure.Memory
{
    public class WindowsMemoryAccessor(
        MemoryMappedFile memoryMappedFile,
        MemoryMappedViewAccessor memoryMappedViewAccessor) : IMemoryAccessor
    {
        public int ReadInt32(long address) => memoryMappedViewAccessor.ReadInt32(address);

        public short ReadInt16(long address) => memoryMappedViewAccessor.ReadInt16(address);

        public void ReadArray(long address, byte[] buffer, int index, int count)
        {
            memoryMappedViewAccessor.ReadArray(address, buffer, index, count);
        }

        public void Dispose()
        {
            memoryMappedViewAccessor.Dispose();
            memoryMappedFile.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
