using System;

namespace Backend.Interfaces
{
    public interface IMemoryAccessor : IDisposable
    {
        int ReadInt32(int address);
        short ReadInt16(int address);
        void ReadArray(int address, byte[] buffer, int index, int count);
    }
}
