namespace Backend.Interfaces
{
    public interface IMemoryReader : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        byte[]? ReadBytes(long address, int length);
        int? ReadInt32(long address);
        short? ReadInt16(long address);
        byte ReadByteSafe(long address, long? bitMask = null);
    }
}
