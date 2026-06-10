namespace Backend.Application;

public interface IDuckstationConnector
{
    DuckstationConnectionStatus getConnectionStatus();
    void HandleProcessingFailure(Exception exception);
    void HandleSilentReadFailure();
}
