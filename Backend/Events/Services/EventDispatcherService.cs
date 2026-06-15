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

        var isConnected = gameStateStore.IsConnectedWithEmulator ?? false;
        SafeDispatch(
            new Event(
                EventType.EmulatorConnectionStatusChanged,
                new ConnectionDTO(
                    isConnected,
                    isConnected ? null : gameStateStore.LastEmulatorConnectionErrorCode,
                    isConnected ? null : gameStateStore.LastEmulatorConnectionErrorDetail)),
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
