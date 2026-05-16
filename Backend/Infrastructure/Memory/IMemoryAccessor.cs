namespace Backend.Infrastructure.Memory
{
    public interface IMemoryAccessor : IDisposable
    {
        int ReadInt32(long address);
        short ReadInt16(long address);
        void ReadArray(long address, byte[] buffer, int index, int count);
    }
}
