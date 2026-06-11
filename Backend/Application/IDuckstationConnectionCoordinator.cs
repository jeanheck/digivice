namespace Backend.Application;

public interface IDuckstationConnectionCoordinator
{
    DuckstationConnectionStatus Handle();
    void HandleProcessingFailure(Exception exception);
    void HandleMemoryReadFailure();
}
