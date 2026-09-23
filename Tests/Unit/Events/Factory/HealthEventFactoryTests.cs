namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;
using Backend.Events.States;
using Moq;
using Xunit;

public class HealthEventFactoryTests
{
    [Fact]
    public void CreateSuccess_ShouldReturnNoEvents_WhenAlreadyHealthy()
    {
        var gameStateStoreMock = new Mock<IGameStateStore>();
        gameStateStoreMock.Setup(store => store.IsHealthy).Returns(true);

        var result = HealthEventFactory.CreateSuccess(gameStateStoreMock.Object);

        Assert.Empty(result);
        gameStateStoreMock.VerifySet(store => store.IsHealthy = It.IsAny<bool>(), Times.Never);
    }

    [Fact]
    public void CreateSuccess_ShouldSetStateAndReturnEvent_WhenHealthy()
    {
        var gameStateStore = new GameStateStore();

        var result = HealthEventFactory.CreateSuccess(gameStateStore).ToList();

        Assert.True(gameStateStore.IsHealthy);
        Assert.Null(gameStateStore.LastErrorCode);
        Assert.Null(gameStateStore.LastErrorDetail);

        var ev = Assert.Single(result);
        Assert.Equal(EventType.HealthChanged, ev.Type);

        var dto = Assert.IsType<HealthDTO>(ev.Payload);
        Assert.True(dto.IsHealthy);
        Assert.Null(dto.ErrorCode);
        Assert.Null(dto.ErrorDetail);
    }

    [Fact]
    public void CreateSuccess_ShouldClearErrorCodes_WhenHealthy()
    {
        var gameStateStore = new GameStateStore
        {
            LastErrorCode = "process_not_found",
            LastErrorDetail = "detail"
        };

        HealthEventFactory.CreateSuccess(gameStateStore);

        Assert.Null(gameStateStore.LastErrorCode);
        Assert.Null(gameStateStore.LastErrorDetail);
    }

    [Fact]
    public void CreateError_ShouldClearStateAndReturnEvent_WhenUnhealthy()
    {
        var gameStateStore = new GameStateStore
        {
            IsHealthy = true
        };

        var result = HealthEventFactory.CreateError(gameStateStore, "process_not_found").ToList();

        Assert.Null(gameStateStore.CurrentState);
        Assert.False(gameStateStore.IsHealthy);
        Assert.Equal("process_not_found", gameStateStore.LastErrorCode);

        var ev = Assert.Single(result);
        Assert.Equal(EventType.HealthChanged, ev.Type);

        var dto = Assert.IsType<HealthDTO>(ev.Payload);
        Assert.False(dto.IsHealthy);
        Assert.Equal("process_not_found", dto.ErrorCode);
    }

    [Fact]
    public void CreateError_ShouldReturnNoEvents_WhenStoreWasAlreadyCleared()
    {
        var gameStateStore = new GameStateStore
        {
            IsHealthy = true
        };
        gameStateStore.UpdateState(new State());
        gameStateStore.ClearState();

        var result = HealthEventFactory.CreateError(gameStateStore, "process_not_found");

        Assert.Empty(result);
    }

    [Fact]
    public void CreateError_ShouldReturnEvent_WhenAlreadyUnhealthyButStateExists()
    {
        var gameStateStore = new GameStateStore
        {
            IsHealthy = false
        };
        gameStateStore.UpdateState(new State());

        var result = HealthEventFactory.CreateError(gameStateStore, "process_not_found").ToList();

        Assert.Null(gameStateStore.CurrentState);
        Assert.False(gameStateStore.IsHealthy);

        var ev = Assert.Single(result);
        var dto = Assert.IsType<HealthDTO>(ev.Payload);
        Assert.False(dto.IsHealthy);
        Assert.Equal("process_not_found", dto.ErrorCode);
    }

    [Fact]
    public void CreateError_ShouldPersistErrorDetail_WhenProvided()
    {
        var gameStateStore = new GameStateStore
        {
            IsHealthy = true
        };

        var result = HealthEventFactory.CreateError(
            gameStateStore,
            "memory_read_failed",
            "Failed to read player data").ToList();

        Assert.Equal("memory_read_failed", gameStateStore.LastErrorCode);
        Assert.Equal("Failed to read player data", gameStateStore.LastErrorDetail);

        var dto = Assert.IsType<HealthDTO>(Assert.Single(result).Payload);
        Assert.Equal("memory_read_failed", dto.ErrorCode);
        Assert.Equal("Failed to read player data", dto.ErrorDetail);
    }
}

