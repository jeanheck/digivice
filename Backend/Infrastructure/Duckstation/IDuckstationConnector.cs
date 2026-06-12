namespace Backend.Infrastructure.Duckstation;

public interface IDuckstationConnector
{
    ConnectionAttemptResult EnsureConnection();
    void ClearSession();
}
