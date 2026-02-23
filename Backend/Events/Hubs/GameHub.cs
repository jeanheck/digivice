using Backend.Events.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Backend.Events.Hubs
{
    /// <summary>
    /// SignalR Hub used as the WebSocket endpoint for client connections.
    /// Clients connect here and receive game state events pushed by the server.
    /// </summary>
    public class GameHub : Hub
    {
        private readonly IEventDispatcherService _dispatcherService;

        public GameHub(IEventDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;
        }

        public override Task OnConnectedAsync()
        {
            _dispatcherService.DispatchInitialStateToClient(Context.ConnectionId);
            return base.OnConnectedAsync();
        }
    }
}
