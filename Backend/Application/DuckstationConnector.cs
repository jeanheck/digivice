using Backend.Diagnostics;
using Backend.Events.Services;
using Backend.Memory.Readers;

namespace Backend.Application;

public class DuckstationConnector(
    IMemoryReader memoryReader,
    IEventDispatcherService eventDispatcherService,
    DebugConsoleRenderer debugConsoleRenderer,
    IConfiguration configuration) : IDuckstationConnector
{
    public DuckstationConnectionStatus EnsureSession()
    {
        if (memoryReader.IsConnected && !memoryReader.IsConnectionAlive())
        {
            MarkDuckstationDisconnected();
            return DuckstationConnectionStatus.SessionLost;
        }

        if (!memoryReader.IsConnected)
        {
            if (!memoryReader.TryConnect())
            {
                eventDispatcherService.DispatchEmulatorConnectionStatus(false);
                return DuckstationConnectionStatus.WaitingForEmulator;
            }

            Serilog.Log.Information("Connected to DuckStation.");
            eventDispatcherService.DispatchEmulatorConnectionStatus(true);
        }

        return DuckstationConnectionStatus.Ready;
    }

    public void HandleProcessingFailure(Exception exception)
    {
        Serilog.Log.Error(exception, "Error processing game state in GameLoopService.");
        MarkDuckstationDisconnected();
    }

    public void HandleSilentReadFailure()
    {
        if (!memoryReader.IsConnected)
        {
            MarkDuckstationDisconnected();
        }
    }

    private void MarkDuckstationDisconnected()
    {
        memoryReader.Disconnect();
        eventDispatcherService.DispatchEmulatorConnectionStatus(false);

        var isDebuggingEnabled = configuration.GetValue<bool>("Features:Debugging");
        if (isDebuggingEnabled && !Console.IsOutputRedirected)
        {
            debugConsoleRenderer.Render(null);
        }
    }
}
