using Backend.Infrastructure.Memory;

namespace Backend.Infrastructure.Duckstation;

public interface IDuckstationConnector : IDisposable
{
    bool IsConnected { get; }
    IMemoryAccessor? Accessor { get; }
    bool IsConnectionAlive();
    bool TryConnect();
    void Disconnect();
    void InvalidateConnection();
}
