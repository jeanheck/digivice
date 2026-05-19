namespace Tests.Application;

using Backend.Application.Loaders;
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
            NameInBytes = "Agumon"u8.ToArray(),
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

    [Fact]
    public void Get_ShouldHandleNullNameInBytes()
    {
        var playerResource = new PlayerResource
        {
            NameInBytes = null,
            Bits = 100,
            MapId = 5
        };

        var playerLoaderMock = new Mock<IPlayerLoader>();
        playerLoaderMock.Setup(loader => loader.Load()).Returns(playerResource);

        var provider = new PlayerProvider(playerLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.Equal(100, result.Bits);
        playerLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldHandleNullBits()
    {
        var playerResource = new PlayerResource
        {
            NameInBytes = "Agumon"u8.ToArray(),
            Bits = null,
            MapId = 2
        };

        var playerLoaderMock = new Mock<IPlayerLoader>();
        playerLoaderMock.Setup(loader => loader.Load()).Returns(playerResource);

        var provider = new PlayerProvider(playerLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.Equal(0, result.Bits);
        playerLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldHandleNullMapId()
    {
        var playerResource = new PlayerResource
        {
            NameInBytes = "Agumon"u8.ToArray(),
            Bits = 150,
            MapId = null
        };

        var playerLoaderMock = new Mock<IPlayerLoader>();
        playerLoaderMock.Setup(loader => loader.Load()).Returns(playerResource);

        var provider = new PlayerProvider(playerLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.MapId);
        playerLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }
}
