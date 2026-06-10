namespace Backend.Application;

public interface IDuckstationConnectionCoordinator
{
    DuckstationConnectionStatus GetConnectionStatus();
    void HandleProcessingFailure(Exception exception);
    bool HandleSilentReadFailure();
}
