namespace Tests.Application;

using Backend.Application.Loaders;
using Backend.Application.Providers;
using Backend.Domain.Models;
using Backend.Memory.Resources;
using Moq;

public class ImportantItemsProviderTests
{
    [Fact]
    public void Get_ShouldLoadResourceAndApplyImportantItemsAssembler()
    {
        var resource = new ImportantItemsResource
        {
            TreeBoots = 0x01,
            FishingPole = 0x00,
            AsukaTrophy = 0x02,
            SunTrophy = 0x00
        };

        var loaderMock = new Mock<IImportantItemsLoader>();
        loaderMock.Setup(loader => loader.Load()).Returns(resource);

        var provider = new ImportantItemsProvider(loaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.IsType<ImportantItems>(result);
        Assert.True(result.TreeBoots);
        Assert.False(result.FishingPole);
        Assert.True(result.AsukaTrophy);
        Assert.False(result.SunTrophy);
        loaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldHandleNullBytesAsFalse()
    {
        var resource = new ImportantItemsResource
        {
            TreeBoots = null,
            FishingPole = null,
            AsukaTrophy = null,
            SunTrophy = null
        };

        var loaderMock = new Mock<IImportantItemsLoader>();
        loaderMock.Setup(loader => loader.Load()).Returns(resource);

        var provider = new ImportantItemsProvider(loaderMock.Object);

        var result = provider.Get();

        Assert.False(result.TreeBoots);
        Assert.False(result.FishingPole);
        Assert.False(result.AsukaTrophy);
        Assert.False(result.SunTrophy);
        loaderMock.Verify(loader => loader.Load(), Times.Once);
    }
}
