namespace Backend.Memory.Readers.Helpers;

public static class FlagByteHelper
{
    public static byte Read(IMemoryReader memoryReader, long address, long? bitMask = null)
    {
        if (address == 0)
        {
            return 0;
        }

        byte rawValue = memoryReader.ReadBytes(address, 1)[0];
        if (bitMask == null)
        {
            return rawValue;
        }

        return (byte)(rawValue & bitMask.Value);
    }
}
