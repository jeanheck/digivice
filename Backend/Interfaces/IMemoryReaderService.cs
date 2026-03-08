namespace Backend.Interfaces
{
    public interface IMemoryReaderService : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        byte[]? ReadBytes(long address, int length);
        int ReadInt32(long address);
        short ReadInt16(long address);
        string ReadString(long address, int length, System.Text.Encoding? encoding = null);
        byte ReadByteSafe(string? hexAddress);
    }
}
