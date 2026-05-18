using Backend.Events.Models;
using Backend.Events.Models.Connection;
using Backend.Events.Models.State;
using Backend.Events.Hubs;
using Backend.Events.States;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Events.Services;

public class EventDispatcherService(
    IHubContext<GameHub> hubContext,
    ILogger<EventDispatcherService> logger,
    IGameStateStore gameStateStore) : IEventDispatcherService
{
    public void DispatchConnectionStatus(bool isConnected)
    {
        if (gameStateStore.IsEmulatorConnected == isConnected)
        {
            return;
        }

        gameStateStore.IsEmulatorConnected = isConnected;
        SafeDispatch(new ConnectionStatusChangedEvent(isConnected));

        if (!isConnected)
        {
            gameStateStore.ClearState();
        }
    }

    public void DispatchInitialStateToClient(string connectionId)
    {
        var currentState = gameStateStore.CurrentState;
        if (currentState != null)
        {
            SafeDispatch(new InitialStateEvent(currentState), hubContext.Clients.Client(connectionId));
        }
    }

    public void DispatchEvents(IEnumerable<BaseEvent> events)
    {
        foreach (var ev in events)
        {
            SafeDispatch(ev);
        }
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
