using Backend.Diagnostics;
using Backend.Events.Interfaces;
using Backend.Interfaces;

namespace Backend.Services
{
    public class GameLoopBackgroundService
    (
        IMemoryReaderService readerService,
        GameStateService gameStateService,
        IEventDispatcherService dispatcherService,
        DebugConsoleRenderer debugConsoleRenderer,
        IConfiguration configuration
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Serilog.Log.Information("Starting GameLoopBackgroundService...");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!readerService.IsConnected)
                {
                    if (!readerService.TryConnect())
                    {
                        // If the connection fails, it sends false and waits 1 second before trying again.
                        dispatcherService.DispatchConnectionStatus(false);
                        await Task.Delay(1000, stoppingToken);
                        continue;
                    }
                    else
                    {
                        // Successful connection.
                        Serilog.Log.Information("Connected to DuckStation.");
                        dispatcherService.DispatchConnectionStatus(true);
                    }
                }

                try
                {
                    var state = gameStateService.GetState();

                    if (state?.Player != null)
                    {
                        // Dispatch any state differences
                        dispatcherService.ProcessGameState(state);

                        // Render debugging console if flag is enabled
                        var isDebuggingEnabled = configuration.GetValue<bool>("Features:Debugging");
                        if (isDebuggingEnabled && !Console.IsOutputRedirected)
                        {
                            debugConsoleRenderer.Render(state);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ignore the error if the application is being shut down.
                    break;
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error(ex, "Error processing game state in GameLoopBackgroundService.");
                    // In case the connection was lost abruptly during read
                    if (!readerService.IsConnected)
                    {
                        dispatcherService.DispatchConnectionStatus(false);
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

            Serilog.Log.Information("GameLoopBackgroundService shutting down gracefully.");
        }
    }
}
