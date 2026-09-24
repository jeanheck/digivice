namespace Tests.Application;

using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers;
using Backend.Domain.Models;
using Backend.Memory.Resources;
using Moq;

public class CardBattleProviderTests
{
    [Fact]
    public void Get_ShouldLoadResourceAndApplyCardBattleAssembler()
    {
        var cardBattleResource = new CardBattleResource
        {
            Id = 7,
        };

        var cardBattleLoaderMock = new Mock<ICardBattleLoader>();
        cardBattleLoaderMock.Setup(loader => loader.Load()).Returns(cardBattleResource);

        var provider = new CardBattleProvider(cardBattleLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.IsType<CardBattle>(result);
        Assert.Equal(7, result.Id);
        cardBattleLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldReturnNullId_WhenResourceIdIsZero()
    {
        var cardBattleResource = new CardBattleResource
        {
            Id = 0,
        };

        var cardBattleLoaderMock = new Mock<ICardBattleLoader>();
        cardBattleLoaderMock.Setup(loader => loader.Load()).Returns(cardBattleResource);

        var provider = new CardBattleProvider(cardBattleLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.Null(result.Id);
        cardBattleLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }
}
