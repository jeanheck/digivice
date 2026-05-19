namespace Tests.Application;

using Backend.Application.Loaders;
using Backend.Application.Providers;
using Backend.Domain.Models;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Parties;
using Moq;

public class PartyProviderTests
{
    [Fact]
    public void Get_ShouldLoadResourceAndApplyPartyAssembler()
    {
        var slotResource = new DigimonSlotResource
        {
            DigimonId = 1,
            DigimonResource = new()
        };

        var partyResource = new PartyResource
        {
            SlotsResource = [slotResource]
        };

        var partyLoaderMock = new Mock<IPartyLoader>();
        partyLoaderMock.Setup(loader => loader.Load()).Returns(partyResource);

        var provider = new PartyProvider(partyLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.IsType<Party>(result);
        Assert.Single(result.Slots);
        partyLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldReturnEmptyPartyWhenNoSlots()
    {
        var partyResource = new PartyResource
        {
            SlotsResource = []
        };

        var partyLoaderMock = new Mock<IPartyLoader>();
        partyLoaderMock.Setup(loader => loader.Load()).Returns(partyResource);

        var provider = new PartyProvider(partyLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.Empty(result.Slots);
        partyLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldReturnMultipleSlots()
    {
        var slotResources = new List<DigimonSlotResource>
        {
            new() { DigimonId = 1, DigimonResource = new() },
            new() { DigimonId = 2, DigimonResource = new() },
            new() { DigimonId = 3, DigimonResource = new() }
        };

        var partyResource = new PartyResource
        {
            SlotsResource = slotResources
        };

        var partyLoaderMock = new Mock<IPartyLoader>();
        partyLoaderMock.Setup(loader => loader.Load()).Returns(partyResource);

        var provider = new PartyProvider(partyLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.Equal(3, result.Slots.Count);
        partyLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }
}
