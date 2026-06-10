using Backend.Events.Services;
using Backend.Events.States;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Events.Hubs;

/// <summary>
/// SignalR Hub used as the WebSocket endpoint for client connections.
/// Clients connect here and receive game state events pushed by the server.
/// </summary>
public class GameHub(
    IEventDispatcherService eventDispatcherService,
    IGameStateStore gameStateStore) : Hub
{
    public override Task OnConnectedAsync()
    {
        gameStateStore.RegisterClientConnection();
        eventDispatcherService.DispatchInitialStateToClient(Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        gameStateStore.UnregisterClientConnection();
        return base.OnDisconnectedAsync(exception);
    }
}

