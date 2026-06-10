namespace Backend.Application;

public interface IDuckstationConnectionCoordinator
{
    DuckstationConnectionStatus GetConnectionStatus();
    void HandleProcessingFailure(Exception exception);
    void HandleMemoryReadFailure();
}
