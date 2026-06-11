namespace Backend.Infrastructure.Duckstation;

public interface IDuckstationConnector
{
    bool EnsureConnection();
    void ClearSession();
}
