namespace Tests.Application;

using Backend.Application.Loaders;
using Backend.Application.Providers;
using Backend.Domain.Models;
using Backend.Memory.Resources;
using Moq;
using Xunit;

public class CardBattleProviderTests
{
    [Fact]
    public void Get_ShouldLoadResourceAndApplyCardBattleAssembler()
    {
        var cardBattleResource = new CardBattleResource
        {
            OpponentId = 7,
        };

        var cardBattleLoaderMock = new Mock<ICardBattleLoader>();
        cardBattleLoaderMock.Setup(loader => loader.Load()).Returns(cardBattleResource);

        var provider = new CardBattleProvider(cardBattleLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.IsType<CardBattle>(result);
        Assert.Equal(7, result.OpponentId);
        cardBattleLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldHandleNullOpponentId()
    {
        var cardBattleResource = new CardBattleResource
        {
            OpponentId = null,
        };

        var cardBattleLoaderMock = new Mock<ICardBattleLoader>();
        cardBattleLoaderMock.Setup(loader => loader.Load()).Returns(cardBattleResource);

        var provider = new CardBattleProvider(cardBattleLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.Equal(0, result.OpponentId);
        cardBattleLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }
}
