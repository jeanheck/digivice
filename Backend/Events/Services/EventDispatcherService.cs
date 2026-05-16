using Backend.Events.Models;
using Backend.Events.Models.Connection;
using Backend.Events.Models.State;
using Backend.Domain.Models;
using Backend.Events.Hubs;
using Backend.Events.Generation;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Events.Services;

public class EventDispatcherService(
    IHubContext<GameHub> hubContext,
    ILogger<EventDispatcherService> logger,
    StateEventGenerator stateEventGenerator) : IEventDispatcherService
{
    private State? previousState;
    private bool? previousConnectionStatus;

    public void DispatchConnectionStatus(bool isConnected)
    {
        if (previousConnectionStatus == isConnected)
        {
            return;
        }

        previousConnectionStatus = isConnected;
        SafeDispatch(new ConnectionStatusChangedEvent(isConnected));

        if (!isConnected)
        {
            // Reset state so that next connection will do a full sync
            previousState = null;
        }
    }

    public void DispatchInitialStateToClient(string connectionId)
    {
        if (previousState != null)
        {
            SafeDispatch(new InitialStateEvent(previousState), hubContext.Clients.Client(connectionId));
        }
    }

    public void ProcessGameState(State newState)
    {
        var events = stateEventGenerator.Generate(previousState, newState);

        foreach (var ev in events)
        {
            SafeDispatch(ev);
        }

        previousState = newState;
    }

    private void SafeDispatch(BaseEvent ev, IClientProxy? target = null)
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
