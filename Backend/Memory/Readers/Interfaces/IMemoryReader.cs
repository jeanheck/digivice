namespace Backend.Memory.Readers.Interfaces
{
    public interface IMemoryReader
    {
        byte[] ReadBytes(long address, int length);
        byte ReadByte(long address);
        byte ReadByte(long address, long bitMask);
        int ReadInt32(long address);
        short ReadInt16(long address);
    }
}
