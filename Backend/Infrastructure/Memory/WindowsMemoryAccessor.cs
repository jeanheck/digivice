using Backend.Interfaces;
using System.IO.MemoryMappedFiles;

namespace Backend.Infrastructure.Memory
{
    public class WindowsMemoryAccessor(MemoryMappedViewAccessor accessor) : IMemoryAccessor
    {
        public int ReadInt32(long address) => accessor.ReadInt32(address);

        public short ReadInt16(long address) => accessor.ReadInt16(address);

        public void ReadArray(long address, byte[] buffer, int index, int count)
        {
            accessor.ReadArray(address, buffer, index, count);
        }

        public void Dispose()
        {
            accessor.Dispose();
        }
    }
}
