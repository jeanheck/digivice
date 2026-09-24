namespace Tests.Application;

using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers;
using Backend.Domain.Models;
using Backend.Memory.Resources;
using Moq;

public class PlayerProviderTests
{
    [Fact]
    public void Get_ShouldLoadResourceAndApplyPlayerAssembler()
    {
        var playerResource = new PlayerResource
        {
            Bits = 250,
            MapId = 1
        };

        var playerLoaderMock = new Mock<IPlayerLoader>();
        playerLoaderMock.Setup(loader => loader.Load()).Returns(playerResource);

        var provider = new PlayerProvider(playerLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.IsType<Player>(result);
        Assert.Equal(250, result.Bits);
        Assert.Equal("0001", result.MapId);
        playerLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }
}
