using Backend.Diagnostics;
using Backend.Events.Services;
using Backend.Infrastructure.Duckstation;

namespace Backend.Application;

public class DuckstationConnectionCoordinator(
    IDuckstationConnector duckstationConnector,
    IEventDispatcherService eventDispatcherService,
    DebugConsoleRenderer debugConsoleRenderer,
    IConfiguration configuration) : IDuckstationConnectionCoordinator
{
    public DuckstationConnectionStatus GetConnectionStatus()
    {
        if (duckstationConnector.IsConnected && !duckstationConnector.IsConnectionAlive())
        {
            MarkDuckstationDisconnected();
            return DuckstationConnectionStatus.ConnectionLost;
        }

        if (!duckstationConnector.IsConnected)
        {
            if (!duckstationConnector.TryConnect())
            {
                eventDispatcherService.DispatchEmulatorConnectionStatus(false);
                return DuckstationConnectionStatus.WaitingForEmulator;
            }

            eventDispatcherService.DispatchEmulatorConnectionStatus(true);
        }

        return DuckstationConnectionStatus.Ready;
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
