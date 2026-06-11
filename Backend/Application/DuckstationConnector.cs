using Backend.Diagnostics;
using Backend.Events.Services;
using Backend.Infrastructure.Duckstation;

namespace Backend.Application;

public class DuckstationConnector(
    IDuckstationConnection duckstationConnection,
    IEventDispatcherService eventDispatcherService,
    DebugConsoleRenderer debugConsoleRenderer,
    IConfiguration configuration) : IDuckstationConnector
{
    public bool EnsureConnection()
    {
        if (duckstationConnection.IsConnected && !duckstationConnection.IsConnectionAlive())
        {
            MarkDuckstationDisconnected();
            return false;
        }

        if (!duckstationConnection.IsConnected)
        {
            if (!duckstationConnection.TryConnect())
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
        duckstationConnection.Disconnect();
        eventDispatcherService.DispatchEmulatorConnectionStatus(false);

        var isDebuggingEnabled = configuration.GetValue<bool>("Features:Debugging");
        if (isDebuggingEnabled && !Console.IsOutputRedirected)
        {
            debugConsoleRenderer.Render(null);
        }
    }
}
