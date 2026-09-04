namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Events.Converters;

public class StateConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapPlayerImportantItemsPartyDigimonBattleAndJournal()
    {
        var state = new State
        {
            Player = new Player { Bits = 100, MapId = "0001" },
            ImportantItems = new ImportantItems { TreeBoots = true, FishingPole = false, AsukaTrophy = true },
            Party = new Party { Slots = [] },
            DigimonBattle = new DigimonBattle(),
            CardBattle = new CardBattle { OpponentId = 0 },
            Auctions = new Auctions { DivineBarrier = true },
            Journal = new Journal { MainQuest = new Quest { Id = "MainQuest" }, SideQuests = [] }
        };

        var dto = StateConverter.ToDTO(state);

        Assert.NotNull(dto.Player);
        Assert.True(dto.Player.Bits.HasValue);
        Assert.Equal(100, dto.Player.Bits.Value);

        Assert.NotNull(dto.ImportantItems);
        Assert.True(dto.ImportantItems.TreeBoots.HasValue);
        Assert.True(dto.ImportantItems.TreeBoots.Value);
        Assert.True(dto.ImportantItems.FishingPole.HasValue);
        Assert.False(dto.ImportantItems.FishingPole.Value);

        Assert.NotNull(dto.Party);
        Assert.True(dto.Party.Slots.HasValue);
        Assert.Empty(dto.Party.Slots.Value!);

        Assert.NotNull(dto.DigimonBattle);
        Assert.True(dto.DigimonBattle.Enemy.HasValue);
        Assert.Equal(0, dto.DigimonBattle.Enemy.Value!.Id.Value);

        Assert.NotNull(dto.CardBattle);
        Assert.True(dto.CardBattle.OpponentId.HasValue);
        Assert.Equal(0, dto.CardBattle.OpponentId.Value);

        Assert.NotNull(dto.Auctions);
        Assert.True(dto.Auctions.DivineBarrier.HasValue);
        Assert.True(dto.Auctions.DivineBarrier.Value);

        Assert.NotNull(dto.Journal);
        Assert.True(dto.Journal.MainQuest.HasValue);
        Assert.Equal("MainQuest", dto.Journal.MainQuest.Value!.Id);
    }

    [Fact]
    public void ToDTO_ShouldPreserveNullEntities()
    {
        var state = new State
        {
            Player = null!,
            ImportantItems = null!,
            Party = null!,
            DigimonBattle = null!,
            CardBattle = null!,
            Auctions = null!,
            Journal = null!
        };

        var dto = StateConverter.ToDTO(state);

        Assert.Null(dto.Player);
        Assert.Null(dto.ImportantItems);
        Assert.Null(dto.Party);
        Assert.Null(dto.DigimonBattle);
        Assert.Null(dto.CardBattle);
        Assert.Null(dto.Auctions);
        Assert.Null(dto.Journal);
    }
}
