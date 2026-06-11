namespace Backend.Application;

public interface IDuckstationConnector
{
    bool EnsureConnection();
    void Disconnect();
}
