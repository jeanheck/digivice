namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Domain.Models.Journals.Quests;
using Backend.Domain.Models.Parties;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;

public class StateEventFactoryTests
{
    [Fact]
    public void Create_ShouldReturnInitialStateEvent_WhenPreviousStateIsNull()
    {
        var newState = CreateBaseState();

        var result = StateEventFactory.Create(null, newState).ToList();

        Assert.Single(result);
        Assert.Equal(EventType.InitialState, result[0].Type);

        var dto = Assert.IsType<StateDTO>(result[0].Payload);
        Assert.NotNull(dto.Player);
        Assert.NotNull(dto.ImportantItems);
        Assert.NotNull(dto.Party);
        Assert.NotNull(dto.DigimonBattle);
        Assert.NotNull(dto.CardBattle);
        Assert.NotNull(dto.Journal);
    }

    [Fact]
    public void Create_ShouldReturnNoEvents_WhenStateHasNoChanges()
    {
        var previousState = CreateBaseState();
        var newState = CreateBaseState();

        var result = StateEventFactory.Create(previousState, newState);

        Assert.Empty(result);
    }

    [Fact]
    public void Create_ShouldReturnPlayerChangedEvent_WhenOnlyPlayerChanges()
    {
        var previousState = CreateBaseState();
        var newState = CreateBaseState();
        newState.Player.Bits = 999;

        var result = StateEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.PlayerChanged, ev.Type);
        Assert.IsType<PlayerDTO>(ev.Payload);
    }

    [Fact]
    public void Create_ShouldReturnImportantItemsChangedEvent_WhenOnlyImportantItemsChange()
    {
        var previousState = CreateBaseState();
        var newState = CreateBaseState();
        newState.ImportantItems.TreeBoots = true;

        var result = StateEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.ImportantItemsChanged, ev.Type);
        Assert.IsType<ImportantItemsDTO>(ev.Payload);
    }

    [Fact]
    public void Create_ShouldReturnPartyChangedEvent_WhenOnlyPartyChanges()
    {
        var previousState = CreateBaseState();
        var newState = CreateBaseState();
        newState.Party.Slots[0].Digimon!.Level = 22;

        var result = StateEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.PartyChanged, ev.Type);
        Assert.IsType<PartyDTO>(ev.Payload);
    }

    [Fact]
    public void Create_ShouldReturnDigimonBattleChangedEvent_WhenOnlyDigimonBattleChanges()
    {
        var previousState = CreateBaseState();
        var newState = CreateBaseState();
        newState.DigimonBattle.Enemy.Speed = 84;

        var result = StateEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.DigimonBattleChanged, ev.Type);
        Assert.IsType<DigimonBattleDTO>(ev.Payload);
    }

    [Fact]
    public void Create_ShouldReturnCardBattleChangedEvent_WhenOnlyCardBattleChanges()
    {
        var previousState = CreateBaseState();
        var newState = CreateBaseState();
        newState.CardBattle.OpponentId = 11;

        var result = StateEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.CardBattleChanged, ev.Type);
        Assert.IsType<CardBattleDTO>(ev.Payload);
    }

    [Fact]
    public void Create_ShouldReturnJournalChangedEvent_WhenOnlyJournalChanges()
    {
        var previousState = CreateBaseState();
        var newState = CreateBaseState();
        newState.Journal.MainQuest.Steps[0].Value = 1;

        var result = StateEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.JournalChanged, ev.Type);
        Assert.IsType<JournalDTO>(ev.Payload);
    }

    [Fact]
    public void Create_ShouldReturnJournalChangedEvent_WhenOnlyAuctionsChange()
    {
        var previousState = CreateBaseState();
        var newState = CreateBaseState();
        newState.Journal.Auctions[0].Value = 0x01;

        var result = StateEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.JournalChanged, ev.Type);
        Assert.IsType<JournalDTO>(ev.Payload);
    }

    [Fact]
    public void Create_ShouldReturnEventsInDomainOrder_WhenMultipleSectionsChange()
    {
        var previousState = CreateBaseState();
        var newState = CreateBaseState();
        newState.Player.Bits = 999;
        newState.ImportantItems.AsukaTrophy = true;
        newState.Party.Slots[0].Digimon!.Level = 22;
        newState.DigimonBattle.Enemy.Speed = 84;
        newState.CardBattle.OpponentId = 11;
        newState.Journal.MainQuest.Steps[0].Value = 1;
        newState.Journal.Auctions[0].Value = 0x01;

        var result = StateEventFactory.Create(previousState, newState).ToList();

        Assert.Equal(6, result.Count);
        Assert.Equal(EventType.PlayerChanged, result[0].Type);
        Assert.Equal(EventType.ImportantItemsChanged, result[1].Type);
        Assert.Equal(EventType.PartyChanged, result[2].Type);
        Assert.Equal(EventType.DigimonBattleChanged, result[3].Type);
        Assert.Equal(EventType.CardBattleChanged, result[4].Type);
        Assert.Equal(EventType.JournalChanged, result[5].Type);
    }

    private static State CreateBaseState()
    {
        return new State
        {
            Player = new Player
            {
                Bits = 100,
                MapId = "0001"
            },
            ImportantItems = new ImportantItems
            {
                TreeBoots = false,
                FishingPole = false,
                AsukaTrophy = false
            },
            Party = new Party
            {
                Slots =
                [
                    new DigimonSlot
                    {
                        Index = 1,
                        DigimonId = 1,
                        Digimon = CreateBaseDigimon()
                    }
                ]
            },
            DigimonBattle = new DigimonBattle(),
            CardBattle = new CardBattle(),
            Journal = new Journal
            {
                MainQuest = new Quest
                {
                    Id = "MainQuest",
                    Steps = [new Step { Number = 1, Value = 0 }],
                    Requisites = []
                },
                SideQuests = [],
                Auctions =
                [
                    new Auction
                    {
                        Id = "divineBarrier",
                        Value = 0x00,
                    }
                ]
            }
        };
    }

    private static Digimon CreateBaseDigimon()
    {
        return new Digimon
        {
            Level = 10,
            Experience = 1000,
            ActiveDigievolutionId = 3,
            HP = new Vital { Current = 100, Max = 100 },
            MP = new Vital { Current = 50, Max = 50 },
            Attributes = new Attributes { Strength = 5, Defense = 5, Spirit = 5, Wisdom = 5, Speed = 5, Charisma = 5 },
            Resistances = new Resistances { Fire = 1, Water = 1, Ice = 1, Wind = 1, Thunder = 1, Machine = 1, Dark = 1 },
            Equipments = new Equipments { Head = 0, Body = 0, Right = 0, Left = 0, Accessory1 = 0, Accessory2 = 0 },
            Digievolutions = []
        };
    }
}
