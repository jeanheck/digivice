namespace Tests.Integration.Application.Loaders.Journals;

using Backend.Application.Loaders.Journals;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Journals;
using Backend.Memory.Readers.Journals.Quests;
using Moq;
using Tests.Integration.Application.Loaders;
using Xunit;

public class QuestLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void LoadMainQuest_ShouldIntegrateQuestAddressesAndReaderPipeline()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B3B6, 0x80)).Returns((byte)0x80);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B3B7, 0x01)).Returns((byte)0x01);

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);

        var mainQuest = questLoader.LoadMainQuest();

        Assert.NotNull(mainQuest);
        Assert.Equal("MainQuest", mainQuest.Id);
        Assert.Empty(mainQuest.Requisites);
        Assert.Equal(43, mainQuest.Steps.Count);
        Assert.Equal(1, mainQuest.Steps[0].Number);
        Assert.Equal(0x80, mainQuest.Steps[0].Value);
        Assert.Equal(2, mainQuest.Steps[1].Number);
        Assert.Equal(0x01, mainQuest.Steps[1].Value);
    }

    [Fact]
    public void LoadSideQuests_ShouldIntegrateSideQuestAddressesAndReaderPipeline()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x00048F42, null)).Returns((byte)1);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x00048F42, 0)).Returns((byte)1);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x04B3B0, 0x04)).Returns((byte)0x04);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x048E57, null)).Returns((byte)1);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x048E09, null)).Returns((byte)1);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x00048DB7, null)).Returns((byte)0);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x048DB5, 0)).Returns((byte)1);

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);

        var sideQuests = questLoader.LoadSideQuests();

        Assert.NotNull(sideQuests);
        Assert.Equal(3, sideQuests.Count);

        var folderBag = sideQuests[0];
        Assert.Equal("FolderBag", folderBag.Id);
        Assert.Empty(folderBag.Requisites);
        Assert.Single(folderBag.Steps);
        Assert.Equal(1, folderBag.Steps[0].Value);

        var treeBoots = sideQuests[1];
        Assert.Equal("TreeBoots", treeBoots.Id);
        Assert.Single(treeBoots.Requisites);
        Assert.Equal("FolderBag", treeBoots.Requisites[0].Id);
        Assert.Equal(1, treeBoots.Requisites[0].Value);
        Assert.Equal(7, treeBoots.Steps.Count);
        Assert.Equal(0x04, treeBoots.Steps[0].Value);

        var fishingPole = sideQuests[2];
        Assert.Equal("FishingPole", fishingPole.Id);
        Assert.Single(fishingPole.Requisites);
        Assert.Equal(3, fishingPole.Steps.Count);
        Assert.Equal(3, fishingPole.Steps[2].Requisites.Count);
        Assert.Equal("BambooSpear", fishingPole.Steps[2].Requisites[0].Id);
        Assert.Equal(1, fishingPole.Steps[2].Requisites[0].Value);
        Assert.Equal("SpiderWeb", fishingPole.Steps[2].Requisites[1].Id);
        Assert.Equal(1, fishingPole.Steps[2].Requisites[1].Value);
        Assert.Equal("RedSnapper", fishingPole.Steps[2].Requisites[2].Id);
        Assert.Equal(0, fishingPole.Steps[2].Requisites[2].Value);
        Assert.Equal(1, fishingPole.Steps[2].Value);
    }
}
