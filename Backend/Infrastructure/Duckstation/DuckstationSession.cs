using Backend.Infrastructure.Memory;

namespace Backend.Infrastructure.Duckstation;

public sealed class DuckstationSession : IDuckstationSession
{
    public IMemoryAccessor? Accessor { get; private set; }

    internal void SetAccessor(IMemoryAccessor accessor)
    {
        Accessor = accessor;
    }

    internal void ClearAccessor()
    {
        Accessor?.Dispose();
        Accessor = null;
    }
}
