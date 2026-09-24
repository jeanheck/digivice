namespace Tests.Application.Loaders;

using Backend.Application.Loaders;
using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Resources.Journals;
using Moq;

public class JournalLoaderTests
{
    [Fact]
    public void Load_ShouldComposeEveryQuestCategoryFromQuestLoader()
    {
        var mainQuest = new QuestResource { Id = "mainQuest" };
        List<QuestResource> sideQuests = [new() { Id = "treeBoots" }];
        List<QuestResource> legendaryWeapons = [new() { Id = "eternally" }];
        List<QuestResource> driAgents = [new() { Id = "driAgentGuilmon" }];
        List<QuestResource> duelIsland = [new() { Id = "asukaTrophy" }];

        var questLoaderMock = new Mock<IQuestLoader>();
        questLoaderMock.Setup(loader => loader.LoadMainQuest()).Returns(mainQuest);
        questLoaderMock.Setup(loader => loader.LoadSideQuests()).Returns(sideQuests);
        questLoaderMock.Setup(loader => loader.LoadLegendaryWeapons()).Returns(legendaryWeapons);
        questLoaderMock.Setup(loader => loader.LoadDriAgents()).Returns(driAgents);
        questLoaderMock.Setup(loader => loader.LoadDuelIsland()).Returns(duelIsland);

        var journalResource = new JournalLoader(questLoaderMock.Object).Load();

        Assert.Same(mainQuest, journalResource.MainQuest);
        Assert.Same(sideQuests, journalResource.SideQuests);
        Assert.Same(legendaryWeapons, journalResource.LegendaryWeapons);
        Assert.Same(driAgents, journalResource.DriAgents);
        Assert.Same(duelIsland, journalResource.DuelIsland);
    }
}
