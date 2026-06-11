using Backend.Infrastructure.Memory;

namespace Backend.Infrastructure.Duckstation;

public sealed class DuckstationSession : IDuckstationSession
{
    public IMemoryAccessor? Accessor { get; set; }
}
