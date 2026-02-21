namespace Backend.Interfaces
{
    public interface IMemoryProvider
    {
        IMemoryAccessor? OpenExisting(string mapName);
    }
}
