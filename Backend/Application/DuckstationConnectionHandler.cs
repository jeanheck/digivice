using Backend.Diagnostics;
using Backend.Events.Services;
using Backend.Infrastructure.Duckstation;

namespace Backend.Application;

public class DuckstationConnectionHandler(
    IDuckstationConnector duckstationConnector,
    IEventDispatcherService eventDispatcherService,
    DebugConsoleRenderer debugConsoleRenderer,
    IConfiguration configuration) : IDuckstationConnectionHandler
{
    public bool Handle()
    {
        if (duckstationConnector.IsConnected && !duckstationConnector.IsConnectionAlive())
        {
            MarkDuckstationDisconnected();
            return false;
        }

        if (!duckstationConnector.IsConnected)
        {
            if (!duckstationConnector.TryConnect())
            {
                eventDispatcherService.DispatchEmulatorConnectionStatus(false);
                return false;
            }

            eventDispatcherService.DispatchEmulatorConnectionStatus(true);
        }

        return true;
    }

    public void HandleProcessingFailure(Exception exception)
    {
        Serilog.Log.Error(exception, "Error processing game state in GameLoopService.");
        MarkDuckstationDisconnected();
    }

    public void HandleMemoryReadFailure()
    {
        MarkDuckstationDisconnected();
    }

    private void MarkDuckstationDisconnected()
    {
        duckstationConnector.Disconnect();
        eventDispatcherService.DispatchEmulatorConnectionStatus(false);

        var isDebuggingEnabled = configuration.GetValue<bool>("Features:Debugging");
        if (isDebuggingEnabled && !Console.IsOutputRedirected)
        {
            debugConsoleRenderer.Render(null);
        }
    }
}
