namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Events.Converters;
using Xunit;

public class CardBattleConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapOpponentIdCorrectly()
    {
        var cardBattle = new CardBattle
        {
            OpponentId = 11,
        };

        var dto = CardBattleConverter.ToDTO(cardBattle);

        Assert.True(dto.OpponentId.HasValue);
        Assert.Equal(11, dto.OpponentId.Value);
    }
}
