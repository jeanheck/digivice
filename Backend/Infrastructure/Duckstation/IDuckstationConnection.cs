using Backend.Infrastructure.Memory;

namespace Backend.Infrastructure.Duckstation;

public interface IDuckstationConnection : IDisposable
{
    bool IsConnected { get; }
    IMemoryAccessor? Accessor { get; }
    bool IsConnectionAlive();
    bool TryConnect();
    void Disconnect();
}
