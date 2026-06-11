using Backend.Diagnostics;
using Backend.Events.Factory;
using Backend.Events.Services;
using Backend.Events.States;
using Backend.Infrastructure.Duckstation;
using Backend.Memory;

namespace Backend.Application
{
    public class GameLoopService
    (
        IDuckstationConnector duckstationConnector,
        StateComposer stateComposer,
        IEventDispatcherService eventDispatcherService,
        IGameStateStore gameStateStore,
        DebugConsoleRenderer debugConsoleRenderer,
        IConfiguration configuration
    ) : BackgroundService
    {
        private void NotifyEmulatorUnavailable(bool isDebuggingEnabled)
        {
            eventDispatcherService.DispatchEmulatorConnectionStatus(false);

            if (isDebuggingEnabled && !Console.IsOutputRedirected)
            {
                debugConsoleRenderer.Render(null);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Serilog.Log.Information("Starting GameLoopService...");

            var pollingIntervalMs = configuration.GetValue<int?>("GameLoop:PollingIntervalMs") ?? 1000;
            var isDebuggingEnabled = configuration.GetValue<bool>("Features:Debugging");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (!duckstationConnector.EnsureConnection())
                    {
                        NotifyEmulatorUnavailable(isDebuggingEnabled);
                        await Task.Delay(pollingIntervalMs, stoppingToken);
                        continue;
                    }

                    eventDispatcherService.DispatchEmulatorConnectionStatus(true);

                    try
                    {
                        var newState = stateComposer.Compose();
                        var previousState = gameStateStore.CurrentState;

                        var events = StateEventFactory.Create(previousState, newState);
                        eventDispatcherService.DispatchEvents(events);

                        gameStateStore.UpdateState(newState);

                        if (isDebuggingEnabled && !Console.IsOutputRedirected)
                        {
                            debugConsoleRenderer.Render(newState);
                        }
                    }
                    catch (MemoryReadException)
                    {
                        duckstationConnector.ClearSession();
                        NotifyEmulatorUnavailable(isDebuggingEnabled);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        Serilog.Log.Error(ex, "Error processing game state in GameLoopService.");
                        duckstationConnector.ClearSession();
                        NotifyEmulatorUnavailable(isDebuggingEnabled);
                    }

                    try
                    {
                        await Task.Delay(pollingIntervalMs, stoppingToken);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                }
            }
            finally
            {
                duckstationConnector.ClearSession();
                Serilog.Log.Information("GameLoopService shutting down gracefully.");
            }
        }
    }
}
