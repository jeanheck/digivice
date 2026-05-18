using Backend.Diagnostics;
using Backend.Events.Factory;
using Backend.Events.Services;
using Backend.Events.States;
using Backend.Memory.Readers;

namespace Backend.Application
{
    public class GameLoopService
    (
        IMemoryReader memoryReader,
        StateComposer stateComposer,
        IEventDispatcherService eventDispatcherService,
        IGameStateStore gameStateStore,
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
                    var newState = stateComposer.Compose();
                    var previousState = gameStateStore.CurrentState;

                    var events = GameEventFactory.Create(previousState, newState);
                    eventDispatcherService.DispatchEvents(events);

                    gameStateStore.UpdateState(newState);

                    var isDebuggingEnabled = configuration.GetValue<bool>("Features:Debugging");
                    if (isDebuggingEnabled && !Console.IsOutputRedirected)
                    {
                        debugConsoleRenderer.Render(newState);
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
