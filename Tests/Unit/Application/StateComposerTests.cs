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
        var importantItems = new ImportantItems { TreeBoots = true };
        var party = new Party { Slots = [] };
        var battle = new Battle();
        var journal = new Journal { MainQuest = new Quest { Id = "MainQuest" }, SideQuests = [] };

        var playerProviderMock = new Mock<IPlayerProvider>();
        var importantItemsProviderMock = new Mock<IImportantItemsProvider>();
        var partyProviderMock = new Mock<IPartyProvider>();
        var battleProviderMock = new Mock<IBattleProvider>();
        var journalProviderMock = new Mock<IJournalProvider>();

        playerProviderMock.Setup(p => p.Get()).Returns(player);
        importantItemsProviderMock.Setup(p => p.Get()).Returns(importantItems);
        partyProviderMock.Setup(p => p.Get()).Returns(party);
        battleProviderMock.Setup(p => p.Get()).Returns(battle);
        journalProviderMock.Setup(p => p.Get()).Returns(journal);

        var composer = new StateComposer(
            playerProviderMock.Object,
            importantItemsProviderMock.Object,
            partyProviderMock.Object,
            battleProviderMock.Object,
            journalProviderMock.Object);

        var state = composer.Compose();

        Assert.Same(player, state.Player);
        Assert.Same(importantItems, state.ImportantItems);
        Assert.Same(party, state.Party);
        Assert.Same(battle, state.Battle);
        Assert.Same(journal, state.Journal);
        playerProviderMock.Verify(p => p.Get(), Times.Once);
        importantItemsProviderMock.Verify(p => p.Get(), Times.Once);
        partyProviderMock.Verify(p => p.Get(), Times.Once);
        battleProviderMock.Verify(p => p.Get(), Times.Once);
        journalProviderMock.Verify(p => p.Get(), Times.Once);
    }
}
