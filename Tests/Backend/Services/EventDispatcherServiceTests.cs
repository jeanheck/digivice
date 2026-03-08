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

    private State CreateTestState(int bits, int hp = 100)
    {
        return new State
        {
            Player = new Player
            {
                Name = "Atsushi",
                Bits = bits
            },
            Party = new Party
            {
                Slots = new List<Digimon?>
                {
                    new Digimon
                    {
                        SlotIndex = 0,
                        BasicInfo = new BasicInfo { Name = "Agumon", CurrentHP = hp, MaxHP = 200 },
                        Attributes = new Attributes { Strength = 50 },
                        Resistances = new Resistances { Fire = 10 },
                        Equipments = new Equipments(),
                        EquippedDigievolutions = new Digievolution?[3]
                    },
                    null
                }
            },
            ImportantItems = new ImportantItems
            {
                FolderBag = new ImportantItem { Id = "FolderBag", Name = "Folder Bag", Has = false },
                TreeBoots = new ImportantItem { Id = "TreeBoots", Name = "Tree Boots", Has = false },
                FishingPole = new ImportantItem { Id = "FishingPole", Name = "Fishing Pole", Has = false },
                RedSnapper = new ImportantItem { Id = "RedSnapper", Name = "Red Snapper", Has = false }
            },
            ConsumableItems = new ConsumableItems
            {
                PowerCharge = new ConsumableItem { Id = "PowerCharge", Name = "Power Charge", Quantity = 0 },
                SpiderWeb = new ConsumableItem { Id = "SpiderWeb", Name = "Spider Web", Quantity = 0 },
                BambooSpear = new ConsumableItem { Id = "BambooSpear", Name = "Bamboo Spear", Quantity = 0 }
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
        var state = CreateTestState(500);

        // Act
        _service.ProcessGameState(state);

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                nameof(EventType.InitialStateSync),
                It.Is<object[]>(args => ((InitialStateSyncEvent)args[0]).InitialState.Player.Bits == 500),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void ProcessGameState_ShouldNotEmitEvents_WhenStateIsIdentical()
    {
        // Arrange
        var state = CreateTestState(500);
        _service.ProcessGameState(state); // Initial sync
        _mockClientProxy.Invocations.Clear();

        // Act
        var identicalState = CreateTestState(500); // Creating a new instance but identical values
        _service.ProcessGameState(identicalState);

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void ProcessGameState_ShouldEmitBitsChangedEvent_WhenBitsChange()
    {
        // Arrange
        var state = CreateTestState(500);
        _service.ProcessGameState(state);
        _mockClientProxy.Invocations.Clear();

        // Act
        var mutatedState = CreateTestState(600);
        _service.ProcessGameState(mutatedState);

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
        var state = CreateTestState(500, hp: 100);
        _service.ProcessGameState(state);
        _mockClientProxy.Invocations.Clear();

        // Act
        var mutatedState = CreateTestState(500, hp: 80); // Digimon took damage
        _service.ProcessGameState(mutatedState);

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
        var state = CreateTestState(500);
        _service.ProcessGameState(state);
        _mockClientProxy.Invocations.Clear();

        // Act - Remove the only digimon mimicking a party size change
        var mutatedState = CreateTestState(500);
        mutatedState.Party.Slots[0] = null;
        _service.ProcessGameState(mutatedState);

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
        var state = CreateTestState(500);
        state.Party.Slots[0].BasicInfo.Level = 10;
        state.Party.Slots[0].BasicInfo.Experience = 1000;
        _service.ProcessGameState(state);
        _mockClientProxy.Invocations.Clear();

        // Act - Increase Level
        var mutatedState = CreateTestState(500);
        mutatedState.Party.Slots[0].BasicInfo.Level = 11;
        mutatedState.Party.Slots[0].BasicInfo.Experience = 1200;
        _service.ProcessGameState(mutatedState);

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
        var state = CreateTestState(500);
        state.Party.Slots[0].BasicInfo.Level = 10;
        state.Party.Slots[0].BasicInfo.Experience = 1000;
        _service.ProcessGameState(state);
        _mockClientProxy.Invocations.Clear();

        // Act - Increase ONLY XP
        var mutatedState = CreateTestState(500);
        mutatedState.Party.Slots[0].BasicInfo.Level = 10;
        mutatedState.Party.Slots[0].BasicInfo.Experience = 1150;
        _service.ProcessGameState(mutatedState);

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

    [Fact]
    public void ProcessGameState_ShouldEmitEquipmentsChangedEvent_WhenEquipmentsChange()
    {
        // Arrange
        var state = CreateTestState(500);
        _service.ProcessGameState(state);
        _mockClientProxy.Invocations.Clear();

        // Act - Change Equipment
        var mutatedState = CreateTestState(500);
        mutatedState.Party.Slots[0].Equipments.RightHand = 157; // Picked up a Dagger
        _service.ProcessGameState(mutatedState);

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                nameof(EventType.DigimonEquipmentsChanged),
                It.Is<object[]>(args => ((DigimonEquipmentsChangedEvent)args[0]).Equipments.RightHand == 157),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void ProcessGameState_ShouldEmitDigievolutionsChangedEvent_WhenDigievolutionsChange()
    {
        // Arrange
        var state = CreateTestState(500);
        _service.ProcessGameState(state);
        _mockClientProxy.Invocations.Clear();

        // Act - Equip a Digievolution
        var mutatedState = CreateTestState(500);
        mutatedState.Party.Slots[0].EquippedDigievolutions[0] = new Digievolution { Id = 367, Level = 1 }; // Equipped Growlmon
        _service.ProcessGameState(mutatedState);

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                nameof(EventType.DigimonDigievolutionsChanged),
                It.Is<object[]>(args => ((DigimonDigievolutionsChangedEvent)args[0]).EquippedDigievolutions[0] != null && ((DigimonDigievolutionsChangedEvent)args[0]).EquippedDigievolutions[0]!.Id == 367),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
