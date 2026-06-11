using Backend.Infrastructure.Duckstation;

namespace Backend.Application;

public class DuckstationConnector(IDuckstationConnection duckstationConnection) : IDuckstationConnector
{
    public bool EnsureConnection()
    {
        if (duckstationConnection.IsConnected && !duckstationConnection.IsConnectionAlive())
        {
            Disconnect();
            return false;
        }

        if (!duckstationConnection.IsConnected)
        {
            if (!duckstationConnection.TryConnect())
            {
                return false;
            }
        }

        return true;
    }

    public void Disconnect()
    {
        duckstationConnection.Disconnect();
    }
}
