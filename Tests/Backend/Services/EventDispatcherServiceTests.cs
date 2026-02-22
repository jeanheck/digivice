using Backend.Events.Data;
using Backend.Events.Data.Digimon;
using Backend.Events.Data.Party;
using Backend.Events.Data.Player;
using Backend.Events.Data.System;
using Backend.Events.Hubs;
using Backend.Events.Services;
using Backend.Models;
using Backend.Models.Digimons;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace Tests.Backend.Services;

public class EventDispatcherServiceTests
{
    private readonly Mock<IHubContext<GameHub>> _mockHubContext;
    private readonly Mock<IClientProxy> _mockClientProxy;
    private readonly EventDispatcherService _service;

    public EventDispatcherServiceTests()
    {
        _mockHubContext = new Mock<IHubContext<GameHub>>();
        _mockClientProxy = new Mock<IClientProxy>();

        // Mock the Clients.All pipeline
        var mockClients = new Mock<IHubClients>();
        mockClients.Setup(c => c.All).Returns(_mockClientProxy.Object);
        _mockHubContext.Setup(h => h.Clients).Returns(mockClients.Object);

        _service = new EventDispatcherService(_mockHubContext.Object);
    }

    private Player CreateTestPlayer(int bits, int hp = 100)
    {
        return new Player
        {
            Name = "Atsushi",
            Bits = bits,
            Party = new Party
            {
                Digimons = new List<Digimon>
                {
                    new Digimon
                    {
                        SlotIndex = 0,
                        BasicInfo = new BasicInfo { Name = "Agumon", CurrentHP = hp, MaxHP = 200 },
                        Attributes = new Attributes { Attack = 50 },
                        Resistances = new Resistances { Fire = 10 }
                    }
                }
            }
        };
    }

    [Fact]
    public void DispatchConnectionStatus_ShouldEmitEvent_WhenStatusChanges()
    {
        // Act
        _service.DispatchConnectionStatus(true);

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                nameof(EventType.ConnectionStatusChanged),
                It.Is<object[]>(args => ((ConnectionStatusChangedEvent)args[0]).IsConnected == true),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void DispatchConnectionStatus_ShouldNotEmitEvent_WhenStatusRemainsSame()
    {
        // Arrange
        _service.DispatchConnectionStatus(true);
        _mockClientProxy.Invocations.Clear();

        // Act
        _service.DispatchConnectionStatus(true); // Call again with same value

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void ProcessGameState_ShouldEmitInitialSyncEvent_OnFirstCall()
    {
        // Arrange
        var player = CreateTestPlayer(500);

        // Act
        _service.ProcessGameState(player);

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                nameof(EventType.InitialStateSync),
                It.Is<object[]>(args => ((InitialStateSyncEvent)args[0]).InitialState.Bits == 500),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void ProcessGameState_ShouldNotEmitEvents_WhenStateIsIdentical()
    {
        // Arrange
        var player = CreateTestPlayer(500);
        _service.ProcessGameState(player); // Initial sync
        _mockClientProxy.Invocations.Clear();

        // Act
        var identicalPlayer = CreateTestPlayer(500); // Creating a new instance but identical values
        _service.ProcessGameState(identicalPlayer);

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void ProcessGameState_ShouldEmitBitsChangedEvent_WhenBitsChange()
    {
        // Arrange
        var player = CreateTestPlayer(500);
        _service.ProcessGameState(player);
        _mockClientProxy.Invocations.Clear();

        // Act
        var mutatedPlayer = CreateTestPlayer(600);
        _service.ProcessGameState(mutatedPlayer);

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                nameof(EventType.PlayerBitsChanged),
                It.Is<object[]>(args => ((PlayerBitsChangedEvent)args[0]).NewBits == 600),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void ProcessGameState_ShouldEmitVitalsChangedEvent_WhenHPChanges()
    {
        // Arrange
        var player = CreateTestPlayer(500, hp: 100);
        _service.ProcessGameState(player);
        _mockClientProxy.Invocations.Clear();

        // Act
        var mutatedPlayer = CreateTestPlayer(500, hp: 80); // Digimon took damage
        _service.ProcessGameState(mutatedPlayer);

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                nameof(EventType.DigimonVitalsChanged),
                It.Is<object[]>(args => ((DigimonVitalsChangedEvent)args[0]).CurrentHP == 80),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void ProcessGameState_ShouldEmitPartyChangedEvent_WhenDigimonListChanges()
    {
        // Arrange
        var player = CreateTestPlayer(500);
        _service.ProcessGameState(player);
        _mockClientProxy.Invocations.Clear();

        // Act - Remove the only digimon mimicking a party size change
        var mutatedPlayer = CreateTestPlayer(500);
        mutatedPlayer.Party.Digimons.Clear();
        _service.ProcessGameState(mutatedPlayer);

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                nameof(EventType.PartySlotsChanged),
                It.Is<object[]>(args => ((PartySlotsChangedEvent)args[0]).NewParty.Count == 0),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void ProcessGameState_ShouldEmitLevelUpEvent_WhenLevelIncreases()
    {
        // Arrange
        var player = CreateTestPlayer(500);
        player.Party.Digimons[0].BasicInfo.Level = 10;
        player.Party.Digimons[0].BasicInfo.Experience = 1000;
        _service.ProcessGameState(player);
        _mockClientProxy.Invocations.Clear();

        // Act - Increase Level
        var mutatedPlayer = CreateTestPlayer(500);
        mutatedPlayer.Party.Digimons[0].BasicInfo.Level = 11;
        mutatedPlayer.Party.Digimons[0].BasicInfo.Experience = 1200;
        _service.ProcessGameState(mutatedPlayer);

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                nameof(EventType.DigimonLevelUp),
                It.Is<object[]>(args => ((DigimonLevelUpEvent)args[0]).NewLevel == 11 && ((DigimonLevelUpEvent)args[0]).OldLevel == 10),
                It.IsAny<CancellationToken>()),
            Times.Once);
        // Should also emit XP gained
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                nameof(EventType.DigimonXpGained),
                It.IsAny<object[]>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void ProcessGameState_ShouldNotEmitLevelUpEvent_WhenOnlyXpIncreases()
    {
        // Arrange
        var player = CreateTestPlayer(500);
        player.Party.Digimons[0].BasicInfo.Level = 10;
        player.Party.Digimons[0].BasicInfo.Experience = 1000;
        _service.ProcessGameState(player);
        _mockClientProxy.Invocations.Clear();

        // Act - Increase ONLY XP
        var mutatedPlayer = CreateTestPlayer(500);
        mutatedPlayer.Party.Digimons[0].BasicInfo.Level = 10;
        mutatedPlayer.Party.Digimons[0].BasicInfo.Experience = 1150;
        _service.ProcessGameState(mutatedPlayer);

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                nameof(EventType.DigimonLevelUp),
                It.IsAny<object[]>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        // Should STILL emit XP gained
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                nameof(EventType.DigimonXpGained),
                It.IsAny<object[]>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
