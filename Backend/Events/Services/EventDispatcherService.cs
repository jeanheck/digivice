using Backend.Events.Data;
using Backend.Events.Hubs;
using Backend.Models;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Events.Services;

public class EventDispatcherService(
    IHubContext<GameHub> hubContext,
    ILogger<EventDispatcherService> logger,
    StateChangeDetector detector) : Interfaces.IEventDispatcherService
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
            SafeDispatch(new InitialStateChangedEvent(previousState), hubContext.Clients.Client(connectionId));
        }
    }

    public void ProcessGameState(State newState)
    {
        var events = detector.DetectChanges(previousState, newState);

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
