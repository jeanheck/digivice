namespace Tests.Application;

using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers;
using Backend.Domain.Models;
using Backend.Memory.Resources;
using Moq;

public class CardInventoryProviderTests
{
    [Fact]
    public void Get_ShouldLoadResourceAndApplyCardInventoryAssembler()
    {
        byte[] quantities = new byte[314];
        quantities[9] = 1;

        var resource = new CardInventoryResource
        {
            Quantities = quantities
        };

        var loaderMock = new Mock<ICardInventoryLoader>();
        loaderMock.Setup(l => l.Load()).Returns(resource);

        var provider = new CardInventoryProvider(loaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.IsType<CardInventory>(result);
        Assert.Equal(1, result.Cards[1009]);
        loaderMock.Verify(l => l.Load(), Times.Once);
    }
}
