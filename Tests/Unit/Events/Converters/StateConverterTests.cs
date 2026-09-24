namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Domain.Models.Parties;
using Backend.Events.Converters;

public class StateConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllStateSections()
    {
        var state = new State
        {
            Player = new Player { Bits = 100, MapId = "0001" },
            ImportantItems = new ImportantItems { TreeBoots = true, FishingPole = false, AsukaTrophy = true },
            Party = new Party
            {
                Slots =
                [
                    new DigimonSlot { Index = 1, DigimonId = 1, Digimon = new Digimon { Level = 5 } },
                    new DigimonSlot { Index = 2, DigimonId = null, Digimon = null },
                    new DigimonSlot { Index = 3, DigimonId = null, Digimon = null }
                ]
            },
            DigimonBattle = new DigimonBattle(),
            CardBattle = new CardBattle { Id = null },
            Auctions = new Auctions { DivineBarrier = true },
            Npcs = new Npcs(),
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
        var slots = dto.Party.Slots.Value!;
        Assert.Equal(3, slots.Count);
        Assert.Equal(1, slots[0].DigimonId.Value);
        Assert.NotNull(slots[0].Digimon.Value);
        Assert.True(slots[1].DigimonId.HasValue);
        Assert.Null(slots[1].DigimonId.Value);
        Assert.True(slots[1].Digimon.HasValue);
        Assert.Null(slots[1].Digimon.Value);

        Assert.NotNull(dto.DigimonBattle);
        Assert.True(dto.DigimonBattle.Enemy.HasValue);
        Assert.Null(dto.DigimonBattle.Enemy.Value);

        Assert.NotNull(dto.CardBattle);
        Assert.True(dto.CardBattle.Id.HasValue);
        Assert.Null(dto.CardBattle.Id.Value);

        Assert.NotNull(dto.Auctions);
        Assert.True(dto.Auctions.DivineBarrier.HasValue);
        Assert.True(dto.Auctions.DivineBarrier.Value);

        Assert.NotNull(dto.Npcs);
        Assert.True(dto.Npcs.Genji.HasValue);

        Assert.NotNull(dto.Journal);
        Assert.True(dto.Journal.MainQuest.HasValue);
        Assert.Equal("MainQuest", dto.Journal.MainQuest.Value!.Id);
    }
}
