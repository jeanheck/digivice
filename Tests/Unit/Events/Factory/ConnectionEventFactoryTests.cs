namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;
using Backend.Events.States;
using Moq;
using Xunit;

public class ConnectionEventFactoryTests
{
    [Fact]
    public void CreateSuccess_ShouldReturnNoEvents_WhenAlreadyConnected()
    {
        var gameStateStoreMock = new Mock<IGameStateStore>();
        gameStateStoreMock.Setup(store => store.IsConnectedWithEmulator).Returns(true);

        var result = ConnectionEventFactory.CreateSuccess(gameStateStoreMock.Object);

        Assert.Empty(result);
        gameStateStoreMock.VerifySet(store => store.IsConnectedWithEmulator = It.IsAny<bool>(), Times.Never);
    }

    [Fact]
    public void CreateSuccess_ShouldSetStateAndReturnEvent_WhenConnected()
    {
        var gameStateStore = new GameStateStore();

        var result = ConnectionEventFactory.CreateSuccess(gameStateStore).ToList();

        Assert.True(gameStateStore.IsConnectedWithEmulator);
        Assert.Null(gameStateStore.LastEmulatorConnectionErrorCode);
        Assert.Null(gameStateStore.LastEmulatorConnectionErrorDetail);

        var ev = Assert.Single(result);
        Assert.Equal(EventType.EmulatorConnectionStatusChanged, ev.Type);

        var dto = Assert.IsType<ConnectionDTO>(ev.Payload);
        Assert.True(dto.IsConnected);
        Assert.Null(dto.ErrorCode);
        Assert.Null(dto.ErrorDetail);
    }

    [Fact]
    public void CreateSuccess_ShouldClearErrorCodes_WhenConnected()
    {
        var gameStateStore = new GameStateStore
        {
            LastEmulatorConnectionErrorCode = "process_not_found",
            LastEmulatorConnectionErrorDetail = "detail"
        };

        ConnectionEventFactory.CreateSuccess(gameStateStore);

        Assert.Null(gameStateStore.LastEmulatorConnectionErrorCode);
        Assert.Null(gameStateStore.LastEmulatorConnectionErrorDetail);
    }

    [Fact]
    public void CreateError_ShouldClearStateAndReturnEvent_WhenDisconnected()
    {
        var gameStateStore = new GameStateStore
        {
            IsConnectedWithEmulator = true
        };

        var result = ConnectionEventFactory.CreateError(gameStateStore, "process_not_found").ToList();

        Assert.Null(gameStateStore.CurrentState);
        Assert.False(gameStateStore.IsConnectedWithEmulator);
        Assert.Equal("process_not_found", gameStateStore.LastEmulatorConnectionErrorCode);

        var ev = Assert.Single(result);
        Assert.Equal(EventType.EmulatorConnectionStatusChanged, ev.Type);

        var dto = Assert.IsType<ConnectionDTO>(ev.Payload);
        Assert.False(dto.IsConnected);
        Assert.Equal("process_not_found", dto.ErrorCode);
    }

    [Fact]
    public void CreateError_ShouldReturnNoEvents_WhenStoreWasAlreadyCleared()
    {
        var gameStateStore = new GameStateStore
        {
            IsConnectedWithEmulator = true
        };
        gameStateStore.UpdateState(new State());
        gameStateStore.ClearState();

        var result = ConnectionEventFactory.CreateError(gameStateStore, "process_not_found");

        Assert.Empty(result);
    }

    [Fact]
    public void CreateError_ShouldReturnEvent_WhenAlreadyDisconnectedButStateExists()
    {
        var gameStateStore = new GameStateStore
        {
            IsConnectedWithEmulator = false
        };
        gameStateStore.UpdateState(new State());

        var result = ConnectionEventFactory.CreateError(gameStateStore, "process_not_found").ToList();

        Assert.Null(gameStateStore.CurrentState);
        Assert.False(gameStateStore.IsConnectedWithEmulator);

        var ev = Assert.Single(result);
        var dto = Assert.IsType<ConnectionDTO>(ev.Payload);
        Assert.False(dto.IsConnected);
        Assert.Equal("process_not_found", dto.ErrorCode);
    }

    [Fact]
    public void CreateError_ShouldPersistErrorDetail_WhenProvided()
    {
        var gameStateStore = new GameStateStore
        {
            IsConnectedWithEmulator = true
        };

        var result = ConnectionEventFactory.CreateError(
            gameStateStore,
            "memory_read_failed",
            "Failed to read player data").ToList();

        Assert.Equal("memory_read_failed", gameStateStore.LastEmulatorConnectionErrorCode);
        Assert.Equal("Failed to read player data", gameStateStore.LastEmulatorConnectionErrorDetail);

        var dto = Assert.IsType<ConnectionDTO>(Assert.Single(result).Payload);
        Assert.Equal("memory_read_failed", dto.ErrorCode);
        Assert.Equal("Failed to read player data", dto.ErrorDetail);
    }
}
