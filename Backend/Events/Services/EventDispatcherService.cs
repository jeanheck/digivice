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

        var isHealthy = gameStateStore.IsHealthy ?? false;
        SafeDispatch(
            new Event(
                EventType.HealthChanged,
                new HealthDTO(
                    isHealthy,
                    isHealthy ? null : gameStateStore.LastErrorCode,
                    isHealthy ? null : gameStateStore.LastErrorDetail)),
            target);
    }

    public void DispatchEvents(IEnumerable<Event> events)
    {
        foreach (var ev in events)
        {
            SafeDispatch(ev);
        }
    }
}
