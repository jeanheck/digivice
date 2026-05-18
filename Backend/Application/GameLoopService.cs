using Backend.Diagnostics;
using Backend.Events.Services;
using Backend.Memory.Readers;

namespace Backend.Application
{
    public class GameLoopService
    (
        IMemoryReader memoryReader,
        StateComposer stateComposer,
        IEventDispatcherService eventDispatcherService,
        IGameStateService gameStateService,
        DebugConsoleRenderer debugConsoleRenderer,
        IConfiguration configuration
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Serilog.Log.Information("Starting GameLoopService...");

            while (!stoppingToken.IsCancellationRequested)
            {
                // Trying to connect at the Duckstation
                if (!memoryReader.IsConnected)
                {
                    if (!memoryReader.TryConnect())
                    {
                        // If the connection fails, it sends false and waits 1 second before trying again.
                        eventDispatcherService.DispatchConnectionStatus(false);
                        await Task.Delay(1000, stoppingToken);
                        continue;
                    }
                    else
                    {
                        // Successful connection.
                        Serilog.Log.Information("Connected to DuckStation.");
                        eventDispatcherService.DispatchConnectionStatus(true);
                    }
                }

                // State Processing
                try
                {
                    var state = stateComposer.Compose();

                    gameStateService.ProcessNewState(state);

                    var isDebuggingEnabled = configuration.GetValue<bool>("Features:Debugging");
                    if (isDebuggingEnabled && !Console.IsOutputRedirected)
                    {
                        debugConsoleRenderer.Render(state);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ignore the error if the application is being shut down.
                    break;
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error(ex, "Error processing game state in GameLoopService.");
                    // In case the connection was lost abruptly during read
                    if (!memoryReader.IsConnected)
                    {
                        eventDispatcherService.DispatchConnectionStatus(false);
                    }
                }

                try
                {
                    await Task.Delay(1000, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // Saída natural do loop ao dar Ctrl+C
                    break;
                }
            }

            Serilog.Log.Information("GameLoopService shutting down gracefully.");
        }
    }
}
