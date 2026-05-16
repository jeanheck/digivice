namespace Backend.Infrastructure.Memory
{
    public interface IMemoryProvider
    {
        IMemoryAccessor? OpenExisting(string mapName);
    }
}
