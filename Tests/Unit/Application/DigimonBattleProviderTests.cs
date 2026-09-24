namespace Tests.Application;

using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Battles;
using Backend.Memory.Resources.Parties.Digimons;
using Moq;

public class DigimonBattleProviderTests
{
    [Fact]
    public void Get_ShouldLoadResourceAndApplyDigimonBattleAssembler()
    {
        var resource = new DigimonBattleResource
        {
            Field = 0x02,
            Enemy = new EnemyResource
            {
                Id = 122,
                GroupId = 201,
                Speed = 84,
                HP = new VitalResource { Current = 600, Max = 672 }
            }
        };

        var loaderMock = new Mock<IDigimonBattleLoader>();
        loaderMock.Setup(loader => loader.Load()).Returns(resource);

        var result = new DigimonBattleProvider(loaderMock.Object).Get();

        Assert.Equal(0x02, result.Field);
        Assert.NotNull(result.Enemy);
        Assert.Equal(122, result.Enemy.Id);
        Assert.Equal(600, result.Enemy.HP.Current);
        loaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldReturnNullEnemy_WhenOutOfBattle()
    {
        var loaderMock = new Mock<IDigimonBattleLoader>();
        loaderMock.Setup(loader => loader.Load()).Returns(new DigimonBattleResource());

        var result = new DigimonBattleProvider(loaderMock.Object).Get();

        Assert.Equal(0, result.Field);
        Assert.Null(result.Enemy);
    }
}
