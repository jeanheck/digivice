namespace Tests.Application;

using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers;
using Backend.Memory.Resources;
using Moq;

public class NpcsProviderTests
{
    [Fact]
    public void Get_ShouldLoadResourceAndApplyNpcsAssembler()
    {
        var resource = new NpcsResource
        {
            Genji = new NpcResource
            {
                Battles =
                [
                    new NpcBattleResource { Id = "first", Value = 0x20 },
                    new NpcBattleResource { Id = "second", Value = 0 },
                ],
            },
        };

        var loaderMock = new Mock<INpcsLoader>();
        loaderMock.Setup(loader => loader.Load()).Returns(resource);

        var result = new NpcsProvider(loaderMock.Object).Get();

        Assert.Equal(2, result.Genji.Battles.Count);
        Assert.True(result.Genji.Battles[0].Won);
        Assert.False(result.Genji.Battles[1].Won);
        Assert.Empty(result.Natsumi.Battles);
        loaderMock.Verify(loader => loader.Load(), Times.Once);
    }
}
