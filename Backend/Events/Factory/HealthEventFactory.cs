using Backend.Events.DTO;
using Backend.Events.Models;
using Backend.Events.States;

namespace Backend.Events.Factory;

public static class HealthEventFactory
{
    public static IEnumerable<Event> CreateSuccess(IGameStateStore gameStateStore)
    {
        if (gameStateStore.IsHealthy == true)
        {
            return [];
        }

        gameStateStore.IsHealthy = true;
        gameStateStore.LastErrorCode = null;
        gameStateStore.LastErrorDetail = null;

        return [Create(true)];
    }

    public static IEnumerable<Event> CreateError(
        IGameStateStore gameStateStore,
        string errorCode,
        string? errorDetail = null)
    {
        var shouldNotifyClients =
            gameStateStore.IsHealthy != false
            || gameStateStore.CurrentState != null;

        gameStateStore.LastErrorCode = errorCode;
        gameStateStore.LastErrorDetail = errorDetail;
        gameStateStore.ClearState();

        if (!shouldNotifyClients)
        {
            return [];
        }

        return [Create(false, errorCode, errorDetail)];
    }

    private static Event Create(
        bool isHealthy,
        string? errorCode = null,
        string? errorDetail = null) =>
        new(EventType.HealthChanged, new HealthDTO(isHealthy, errorCode, errorDetail));
}
