using Backend.Diagnostics;
using Backend.Events.Factory;
using Backend.Events.Services;
using Backend.Events.States;
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
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Serilog.Log.Information("Starting GameLoopService...");

            var pollingIntervalMs = configuration.GetValue<int?>("GameLoop:PollingIntervalMs") ?? 1000;
            var isDebuggingEnabled = configuration.GetValue<bool>("Features:Debugging");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!duckstationConnector.EnsureConnection())
                {
                    await Task.Delay(pollingIntervalMs, stoppingToken);
                    continue;
                }

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
                    duckstationConnector.HandleMemoryReadFailure();
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    duckstationConnector.HandleProcessingFailure(ex);
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

            Serilog.Log.Information("GameLoopService shutting down gracefully.");
        }
    }
}
