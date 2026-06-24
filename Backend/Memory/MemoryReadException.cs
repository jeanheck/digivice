namespace Backend.Memory;

public sealed class MemoryReadException : Exception
{
    public long Address { get; }

    public MemoryReadException(long address, string message)
        : base(message)
    {
        Address = address;
    }

    public MemoryReadException(long address, string message, Exception innerException)
        : base(message, innerException)
    {
        Address = address;
    }
}
