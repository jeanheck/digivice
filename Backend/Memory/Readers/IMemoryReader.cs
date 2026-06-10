namespace Backend.Memory.Readers
{
    public interface IMemoryReader
    {
        byte[]? ReadBytes(long address, int length);
        int? ReadInt32(long address);
        short? ReadInt16(long address);
        byte ReadByteSafe(long address, long? bitMask = null);
    }
}
