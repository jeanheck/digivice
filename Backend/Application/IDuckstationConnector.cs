namespace Backend.Application;

public interface IDuckstationConnector
{
    DuckstationConnectionStatus EnsureSession();
    void HandleProcessingFailure(Exception exception);
    void HandleSilentReadFailure();
}
