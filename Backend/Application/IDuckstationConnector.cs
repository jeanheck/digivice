namespace Backend.Application;

public interface IDuckstationConnector
{
    bool EnsureConnection();
    void HandleProcessingFailure(Exception exception);
    void HandleMemoryReadFailure();
}
