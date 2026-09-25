namespace Tests.Application;

using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers;
using Backend.Memory.Resources;
using Moq;

public class AuctionsProviderTests
{
    [Fact]
    public void Get_ShouldLoadResourceAndApplyAuctionsAssembler()
    {
        var resource = new AuctionsResource
        {
            DivineBarrier = 0x01,
            HazardShield = 0x00,
            SniperShield = 0x04,
            DramonShield = 0x00,
            YinYangWand = 0x10,
        };

        var loaderMock = new Mock<IAuctionsLoader>();
        loaderMock.Setup(loader => loader.Load()).Returns(resource);

        var provider = new AuctionsProvider(loaderMock.Object);
        var result = provider.Get();

        Assert.True(result.DivineBarrier);
        Assert.False(result.HazardShield);
        Assert.True(result.SniperShield);
        Assert.False(result.DramonShield);
        Assert.True(result.YinYangWand);
        loaderMock.Verify(loader => loader.Load(), Times.Once);
    }
}
