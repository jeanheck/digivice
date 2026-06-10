using Backend.Events.Converters;
using Backend.Events.DTO;
using Backend.Events.Hubs;
using Backend.Events.Models;
using Backend.Events.States;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Events.Services;

public class EventDispatcherService(
    IHubContext<GameHub> hubContext,
    ILogger<EventDispatcherService> logger,
    IGameStateStore gameStateStore) : IEventDispatcherService
{
    public void DispatchEmulatorConnectionStatus(bool isConnectedWithEmulator)
    {
        if (!isConnectedWithEmulator)
        {
            var shouldNotifyClients =
                gameStateStore.IsConnectedWithEmulator != isConnectedWithEmulator
                || gameStateStore.CurrentState != null;

            gameStateStore.ClearState();

            if (shouldNotifyClients)
            {
                SafeDispatch(new Event(EventType.EmulatorConnectionStatusChanged, new ConnectionDTO(isConnectedWithEmulator)));
            }

            return;
        }

        if (gameStateStore.IsConnectedWithEmulator == isConnectedWithEmulator)
        {
            return;
        }

        gameStateStore.IsConnectedWithEmulator = isConnectedWithEmulator;
        SafeDispatch(new Event(EventType.EmulatorConnectionStatusChanged, new ConnectionDTO(isConnectedWithEmulator)));
    }

    public void DispatchInitialStateToClient(string connectionId)
    {
        var target = hubContext.Clients.Client(connectionId);
        var currentState = gameStateStore.CurrentState;

        if (currentState != null)
        {
            var stateDto = StateConverter.ToDTO(currentState);
            var initialEvent = new Event(EventType.InitialState, stateDto);
            SafeDispatch(initialEvent, target);
        }

        SafeDispatch(
            new Event(EventType.EmulatorConnectionStatusChanged, new ConnectionDTO(gameStateStore.IsConnectedWithEmulator ?? false)),
            target);
    }

    public void DispatchEvents(IEnumerable<Event> events)
    {
        foreach (var ev in events)
        {
            SafeDispatch(ev);
        }
    }

    private void SafeDispatch(Event ev, IClientProxy? target = null)
    {
        target ??= hubContext.Clients.All;
        _ = target.SendAsync(ev.Type.ToString(), ev)
            .ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    logger.LogError(t.Exception, "Error dispatching event {Type}", ev.Type);
                }
            });
    }
}
