using Backend.Events.DTO;
using Backend.Events.Models;
using Backend.Events.States;

namespace Backend.Events.Factory;

public static class HealthEventFactory
{
    public static IEnumerable<Event> CreateSuccess(IGameStateStore gameStateStore)
    {
        var targetStatus = gameStateStore.CurrentState != null
            ? HealthStatus.Healthy
            : HealthStatus.Loading;

        gameStateStore.LastErrorCode = null;
        gameStateStore.LastErrorDetail = null;

        if (gameStateStore.Status == targetStatus)
        {
            return [];
        }

        gameStateStore.Status = targetStatus;
        return [Create(targetStatus)];
    }

    public static IEnumerable<Event> CreateError(
        IGameStateStore gameStateStore,
        string errorCode,
        string? errorDetail = null)
    {
        var shouldNotifyClients =
            gameStateStore.Status != HealthStatus.Error
            || gameStateStore.CurrentState != null;

        gameStateStore.LastErrorCode = errorCode;
        gameStateStore.LastErrorDetail = errorDetail;
        gameStateStore.ClearState();
        gameStateStore.Status = HealthStatus.Error;

        if (!shouldNotifyClients)
        {
            return [];
        }

        return [Create(HealthStatus.Error, errorCode, errorDetail)];
    }

    private static Event Create(
        HealthStatus status,
        string? errorCode = null,
        string? errorDetail = null) =>
        new(EventType.HealthChanged, new HealthDTO(status, errorCode, errorDetail));
}
