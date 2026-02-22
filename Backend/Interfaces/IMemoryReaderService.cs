using System;

namespace Backend.Interfaces
{
    public interface IMemoryReaderService : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        byte[]? ReadBytes(int address, int length);
        int ReadInt32(int address);
        short ReadInt16(int address);
        string ReadString(int address, int length, System.Text.Encoding? encoding = null);
    }
}
