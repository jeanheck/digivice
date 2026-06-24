using Backend.Infrastructure.Memory;

namespace Backend.Infrastructure.Duckstation;

public interface IDuckstationSession
{
    IMemoryAccessor? Accessor { get; }
}
