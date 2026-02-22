using Microsoft.AspNetCore.SignalR;

namespace Backend.Events.Hubs
{
    /// <summary>
    /// SignalR Hub used as the WebSocket endpoint for client connections.
    /// Clients connect here and receive game state events pushed by the server.
    /// </summary>
    public class GameHub : Hub { }
}
