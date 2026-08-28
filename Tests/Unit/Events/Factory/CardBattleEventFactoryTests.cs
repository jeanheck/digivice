namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;
using Xunit;

public class CardBattleEventFactoryTests
{
    [Fact]
    public void Create_ShouldReturnCardBattleChangedEvent_WhenOpponentIdChanges()
    {
        var previousState = new State
        {
            CardBattle = new CardBattle { OpponentId = 0 },
        };
        var newState = new State
        {
            CardBattle = new CardBattle { OpponentId = 1 },
        };

        var result = CardBattleEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.CardBattleChanged, ev.Type);
        var dto = Assert.IsType<CardBattleDTO>(ev.Payload);
        Assert.True(dto.OpponentId.HasValue);
        Assert.Equal(1, dto.OpponentId.Value);
    }

    [Fact]
    public void Create_ShouldReturnNoEvents_WhenCardBattleHasNoChanges()
    {
        var previousState = new State
        {
            CardBattle = new CardBattle { OpponentId = 5 },
        };
        var newState = new State
        {
            CardBattle = new CardBattle { OpponentId = 5 },
        };

        var result = CardBattleEventFactory.Create(previousState, newState);

        Assert.Empty(result);
    }
}
