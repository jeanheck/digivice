namespace Tests.Application;

using Backend.Application;
using Backend.Application.Providers;
using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Moq;
using Xunit;

public class StateComposerTests
{
    [Fact]
    public void Compose_ShouldReturnStateWithProviderResults()
    {
        var player = new Player { Bits = 123, MapId = "0001" };
        var party = new Party { Slots = [] };
        var journal = new Journal { MainQuest = new Quest { Id = "MainQuest" }, SideQuests = [] };

        var playerProviderMock = new Mock<IPlayerProvider>();
        var partyProviderMock = new Mock<IPartyProvider>();
        var journalProviderMock = new Mock<IJournalProvider>();

        playerProviderMock.Setup(p => p.Get()).Returns(player);
        partyProviderMock.Setup(p => p.Get()).Returns(party);
        journalProviderMock.Setup(p => p.Get()).Returns(journal);

        var composer = new StateComposer(
            playerProviderMock.Object,
            partyProviderMock.Object,
            journalProviderMock.Object);

        var state = composer.Compose();

        Assert.Same(player, state.Player);
        Assert.Same(party, state.Party);
        Assert.Same(journal, state.Journal);
        playerProviderMock.Verify(p => p.Get(), Times.Once);
        partyProviderMock.Verify(p => p.Get(), Times.Once);
        journalProviderMock.Verify(p => p.Get(), Times.Once);
    }
}
