namespace Backend.Application;

public interface IDuckstationConnectionHandler
{
    bool Handle();
    void HandleProcessingFailure(Exception exception);
    void HandleMemoryReadFailure();
}
