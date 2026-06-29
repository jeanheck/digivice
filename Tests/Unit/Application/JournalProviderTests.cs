namespace Tests.Application;

using Backend.Application.Loaders;
using Backend.Application.Providers;
using Backend.Domain.Models;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Journals;
using Backend.Memory.Resources.Journals.Quests;
using Moq;

public class JournalProviderTests
{
    [Fact]
    public void Get_ShouldLoadResourceAndApplyJournalAssembler()
    {
        var mainQuestResource = new QuestResource
        {
            Id = "MainQuest",
            Requisites = [],
            Steps = []
        };

        var journalResource = new JournalResource
        {
            MainQuest = mainQuestResource,
            SideQuests = []
        };

        var journalLoaderMock = new Mock<IJournalLoader>();
        journalLoaderMock.Setup(loader => loader.Load()).Returns(journalResource);

        var provider = new JournalProvider(journalLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.IsType<Journal>(result);
        Assert.NotNull(result.MainQuest);
        Assert.Empty(result.SideQuests);
        journalLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldLoadMultipleSideQuests()
    {
        var mainQuestResource = new QuestResource
        {
            Id = "MainQuest",
            Requisites = [],
            Steps = []
        };

        var sideQuestResources = new List<QuestResource>
        {
            new() { Id = "SideQuest1", Requisites = [], Steps = [] },
            new() { Id = "SideQuest2", Requisites = [], Steps = [] }
        };

        var journalResource = new JournalResource
        {
            MainQuest = mainQuestResource,
            SideQuests = sideQuestResources
        };

        var journalLoaderMock = new Mock<IJournalLoader>();
        journalLoaderMock.Setup(loader => loader.Load()).Returns(journalResource);

        var provider = new JournalProvider(journalLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.Equal(2, result.SideQuests.Count);
        journalLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldNormalizeMainQuestProgression()
    {
        var stepResources = new List<StepResource>
        {
            new() { Number = 0, Value = 0 },
            new() { Number = 1, Value = 0 },
            new() { Number = 2, Value = 1 }
        };

        var mainQuestResource = new QuestResource
        {
            Id = "MainQuest",
            Steps = stepResources
        };

        var journalResource = new JournalResource
        {
            MainQuest = mainQuestResource,
            SideQuests = []
        };

        var journalLoaderMock = new Mock<IJournalLoader>();
        journalLoaderMock.Setup(loader => loader.Load()).Returns(journalResource);

        var provider = new JournalProvider(journalLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.NotNull(result.MainQuest);
        Assert.Equal(3, result.MainQuest.Steps.Count);
        Assert.Equal(1, result.MainQuest.Steps[0].Value);
        Assert.Equal(1, result.MainQuest.Steps[1].Value);
        Assert.Equal(1, result.MainQuest.Steps[2].Value);
        journalLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldNotNormalizeWhenAllStepsAreZero()
    {
        var stepResources = new List<StepResource>
        {
            new() { Number = 0, Value = 0 },
            new() { Number = 1, Value = 0 },
            new() { Number = 2, Value = 0 }
        };

        var mainQuestResource = new QuestResource
        {
            Id = "MainQuest",
            Steps = stepResources
        };

        var journalResource = new JournalResource
        {
            MainQuest = mainQuestResource,
            SideQuests = []
        };

        var journalLoaderMock = new Mock<IJournalLoader>();
        journalLoaderMock.Setup(loader => loader.Load()).Returns(journalResource);

        var provider = new JournalProvider(journalLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.NotNull(result.MainQuest);
        Assert.All(result.MainQuest.Steps, step => Assert.Equal(0, step.Value));
        journalLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldNormalizePartialProgression()
    {
        var stepResources = new List<StepResource>
        {
            new() { Number = 0, Value = 0 },
            new() { Number = 1, Value = 1 },
            new() { Number = 2, Value = 0 },
            new() { Number = 3, Value = 0 }
        };

        var mainQuestResource = new QuestResource
        {
            Id = "MainQuest",
            Steps = stepResources
        };

        var journalResource = new JournalResource
        {
            MainQuest = mainQuestResource,
            SideQuests = []
        };

        var journalLoaderMock = new Mock<IJournalLoader>();
        journalLoaderMock.Setup(loader => loader.Load()).Returns(journalResource);

        var provider = new JournalProvider(journalLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.NotNull(result.MainQuest);
        Assert.Equal(1, result.MainQuest.Steps[0].Value);
        Assert.Equal(1, result.MainQuest.Steps[1].Value);
        Assert.Equal(0, result.MainQuest.Steps[2].Value);
        Assert.Equal(0, result.MainQuest.Steps[3].Value);
        journalLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }

    [Fact]
    public void Get_ShouldHandleEmptySteps()
    {
        var mainQuestResource = new QuestResource
        {
            Id = "MainQuest",
            Requisites = [],
            Steps = []
        };

        var journalResource = new JournalResource
        {
            MainQuest = mainQuestResource,
            SideQuests = []
        };

        var journalLoaderMock = new Mock<IJournalLoader>();
        journalLoaderMock.Setup(loader => loader.Load()).Returns(journalResource);

        var provider = new JournalProvider(journalLoaderMock.Object);

        var result = provider.Get();

        Assert.NotNull(result);
        Assert.NotNull(result.MainQuest);
        Assert.Empty(result.MainQuest.Steps);
        journalLoaderMock.Verify(loader => loader.Load(), Times.Once);
    }
}
