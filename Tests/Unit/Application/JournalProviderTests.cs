namespace Tests.Application;

using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Journals;
using Backend.Memory.Resources.Journals.Quests;
using Moq;

public class JournalProviderTests
{
    [Fact]
    public void Get_ShouldLoadResourceAndApplyJournalAssembler()
    {
        var journalResource = new JournalResource
        {
            MainQuest = new QuestResource
            {
                Id = "mainQuest",
                Steps = [
                    new StepResource { Number = 1, Value = 0 },
                    new StepResource { Number = 2, Value = 1 }
                ]
            },
            SideQuests = [new QuestResource { Id = "treeBoots" }]
        };

        var journalLoaderMock = new Mock<IJournalLoader>();
        journalLoaderMock.Setup(loader => loader.Load()).Returns(journalResource);

        var result = new JournalProvider(journalLoaderMock.Object).Get();

        Assert.Equal("mainQuest", result.MainQuest.Id);
        Assert.All(result.MainQuest.Steps, step => Assert.True(step.IsDone));
        Assert.Equal("treeBoots", Assert.Single(result.SideQuests).Id);
        journalLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }
}
