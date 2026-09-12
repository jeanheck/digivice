namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Events.Converters;
using Xunit;

public class CardBattleConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapIdCorrectly()
    {
        var cardBattle = new CardBattle
        {
            Id = 11,
        };

        var dto = CardBattleConverter.ToDTO(cardBattle);

        Assert.True(dto.Id.HasValue);
        Assert.Equal(11, dto.Id.Value);
    }
}
