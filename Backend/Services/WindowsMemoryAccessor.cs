using Backend.Interfaces;
using System.IO.MemoryMappedFiles;

namespace Backend.Services
{
    public class WindowsMemoryAccessor : IMemoryAccessor
    {
        private readonly MemoryMappedViewAccessor _accessor;

        public WindowsMemoryAccessor(MemoryMappedViewAccessor accessor)
        {
            _accessor = accessor;
        }

        public int ReadInt32(int address) => _accessor.ReadInt32(address);

        public short ReadInt16(int address) => _accessor.ReadInt16(address);

        public void ReadArray(int address, byte[] buffer, int index, int count)
        {
            _accessor.ReadArray(address, buffer, index, count);
        }

        public void Dispose()
        {
            _accessor.Dispose();
        }
    }
}
