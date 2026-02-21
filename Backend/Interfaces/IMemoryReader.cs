using System;

namespace Backend.Interfaces
{
    public interface IMemoryReader : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        byte[]? ReadBytes(int address, int length);
    }
}
