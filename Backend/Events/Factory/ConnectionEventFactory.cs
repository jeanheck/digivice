using Backend.Events.DTO;
using Backend.Events.Models;
using Backend.Events.States;

namespace Backend.Events.Factory;

public static class ConnectionEventFactory
{
    public static IEnumerable<Event> CreateSuccess(IGameStateStore gameStateStore)
    {
        if (gameStateStore.IsConnectedWithEmulator == true)
        {
            return [];
        }

        gameStateStore.IsConnectedWithEmulator = true;
        gameStateStore.LastEmulatorConnectionErrorCode = null;
        gameStateStore.LastEmulatorConnectionErrorDetail = null;

        return [Create(true)];
    }

    public static IEnumerable<Event> CreateError(
        IGameStateStore gameStateStore,
        string errorCode,
        string? errorDetail = null)
    {
        var shouldNotifyClients =
            gameStateStore.IsConnectedWithEmulator != false
            || gameStateStore.CurrentState != null;

        gameStateStore.LastEmulatorConnectionErrorCode = errorCode;
        gameStateStore.LastEmulatorConnectionErrorDetail = errorDetail;
        gameStateStore.ClearState();

        if (!shouldNotifyClients)
        {
            return [];
        }

        return [Create(false, errorCode, errorDetail)];
    }

    private static Event Create(
        bool isConnected,
        string? errorCode = null,
        string? errorDetail = null) =>
        new(EventType.EmulatorConnectionStatusChanged, new ConnectionDTO(isConnected, errorCode, errorDetail));
}
