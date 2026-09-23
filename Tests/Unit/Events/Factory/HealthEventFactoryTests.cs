namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;
using Backend.Events.States;
using Xunit;

public class HealthEventFactoryTests
{
    [Fact]
    public void CreateSuccess_ShouldReturnNoEvents_WhenAlreadyLoadingWithoutState()
    {
        var gameStateStore = new GameStateStore();

        var result = HealthEventFactory.CreateSuccess(gameStateStore);

        Assert.Empty(result);
        Assert.Equal(HealthStatus.Loading, gameStateStore.Status);
    }

    [Fact]
    public void CreateSuccess_ShouldSetHealthy_WhenStateExists()
    {
        var gameStateStore = new GameStateStore();
        gameStateStore.UpdateState(new State());

        var result = HealthEventFactory.CreateSuccess(gameStateStore).ToList();

        Assert.Equal(HealthStatus.Healthy, gameStateStore.Status);
        Assert.Null(gameStateStore.LastErrorCode);
        Assert.Null(gameStateStore.LastErrorDetail);

        var ev = Assert.Single(result);
        Assert.Equal(EventType.HealthChanged, ev.Type);

        var dto = Assert.IsType<HealthDTO>(ev.Payload);
        Assert.Equal(HealthStatus.Healthy, dto.Status);
        Assert.Null(dto.ErrorCode);
        Assert.Null(dto.ErrorDetail);
    }

    [Fact]
    public void CreateSuccess_ShouldTransitionFromErrorToLoading_WhenNoState()
    {
        var gameStateStore = new GameStateStore
        {
            Status = HealthStatus.Error,
            LastErrorCode = "process_not_found",
            LastErrorDetail = "detail"
        };

        var result = HealthEventFactory.CreateSuccess(gameStateStore).ToList();

        Assert.Equal(HealthStatus.Loading, gameStateStore.Status);
        Assert.Null(gameStateStore.LastErrorCode);
        Assert.Null(gameStateStore.LastErrorDetail);

        var dto = Assert.IsType<HealthDTO>(Assert.Single(result).Payload);
        Assert.Equal(HealthStatus.Loading, dto.Status);
    }

    [Fact]
    public void CreateError_ShouldClearStateAndReturnEvent_WhenLeavingNonError()
    {
        var gameStateStore = new GameStateStore
        {
            Status = HealthStatus.Loading
        };

        var result = HealthEventFactory.CreateError(gameStateStore, "process_not_found").ToList();

        Assert.Null(gameStateStore.CurrentState);
        Assert.Equal(HealthStatus.Error, gameStateStore.Status);
        Assert.Equal("process_not_found", gameStateStore.LastErrorCode);

        var ev = Assert.Single(result);
        Assert.Equal(EventType.HealthChanged, ev.Type);

        var dto = Assert.IsType<HealthDTO>(ev.Payload);
        Assert.Equal(HealthStatus.Error, dto.Status);
        Assert.Equal("process_not_found", dto.ErrorCode);
    }

    [Fact]
    public void CreateError_ShouldReturnNoEvents_WhenStoreWasAlreadyErrorWithoutState()
    {
        var gameStateStore = new GameStateStore
        {
            Status = HealthStatus.Error
        };

        var result = HealthEventFactory.CreateError(gameStateStore, "process_not_found");

        Assert.Empty(result);
    }

    [Fact]
    public void CreateError_ShouldReturnEvent_WhenAlreadyErrorButStateExists()
    {
        var gameStateStore = new GameStateStore
        {
            Status = HealthStatus.Error
        };
        gameStateStore.UpdateState(new State());

        var result = HealthEventFactory.CreateError(gameStateStore, "process_not_found").ToList();

        Assert.Null(gameStateStore.CurrentState);
        Assert.Equal(HealthStatus.Error, gameStateStore.Status);

        var ev = Assert.Single(result);
        var dto = Assert.IsType<HealthDTO>(ev.Payload);
        Assert.Equal(HealthStatus.Error, dto.Status);
        Assert.Equal("process_not_found", dto.ErrorCode);
    }

    [Fact]
    public void CreateError_ShouldPersistErrorDetail_WhenProvided()
    {
        var gameStateStore = new GameStateStore
        {
            Status = HealthStatus.Healthy
        };

        var result = HealthEventFactory.CreateError(
            gameStateStore,
            "memory_read_failed",
            "Failed to read player data").ToList();

        Assert.Equal("memory_read_failed", gameStateStore.LastErrorCode);
        Assert.Equal("Failed to read player data", gameStateStore.LastErrorDetail);

        var dto = Assert.IsType<HealthDTO>(Assert.Single(result).Payload);
        Assert.Equal(HealthStatus.Error, dto.Status);
        Assert.Equal("memory_read_failed", dto.ErrorCode);
        Assert.Equal("Failed to read player data", dto.ErrorDetail);
    }
}
