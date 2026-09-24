namespace Tests.Application;

using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Parties;
using Moq;

public class PartyProviderTests
{
    private const int EmptySlotId = 0xFF;

    [Fact]
    public void Get_ShouldLoadResourceAndApplyPartyAssembler()
    {
        var partyResource = new PartyResource
        {
            SlotsResource =
            [
                new DigimonSlotResource { Index = 1, DigimonId = 1, DigimonResource = new() { Level = 12 } },
                new DigimonSlotResource { Index = 2, DigimonId = EmptySlotId, DigimonResource = null },
                new DigimonSlotResource { Index = 3, DigimonId = EmptySlotId, DigimonResource = null }
            ]
        };

        var partyLoaderMock = new Mock<IPartyLoader>();
        partyLoaderMock.Setup(loader => loader.Load()).Returns(partyResource);

        var result = new PartyProvider(partyLoaderMock.Object).Get();

        Assert.Equal(3, result.Slots.Count);
        Assert.Equal(1, result.Slots[0].DigimonId);
        Assert.Equal(12, result.Slots[0].Digimon!.Level);
        Assert.Null(result.Slots[1].DigimonId);
        Assert.Null(result.Slots[2].DigimonId);
        partyLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }
}
