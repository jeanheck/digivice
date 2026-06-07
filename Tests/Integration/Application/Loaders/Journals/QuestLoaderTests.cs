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
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x04B3B0, 0xE0)).Returns((byte)0xA0);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x04B3B0, 0xC0)).Returns((byte)0x80);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x048E57, null)).Returns((byte)1);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x048E09, null)).Returns((byte)1);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x00048DB7, null)).Returns((byte)0);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x048DB5, null)).Returns((byte)1);

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);

        var sideQuests = questLoader.LoadSideQuests();

        Assert.NotNull(sideQuests);
        Assert.Equal(3, sideQuests.Count);

        var folderBag = sideQuests[0];
        Assert.Equal("folderBag", folderBag.Id);
        Assert.Empty(folderBag.Requisites);
        Assert.Single(folderBag.Steps);
        Assert.Equal(1, folderBag.Steps[0].Value);

        var treeBoots = sideQuests[1];
        Assert.Equal("treeBoots", treeBoots.Id);
        Assert.Single(treeBoots.Requisites);
        Assert.Equal("folderBag", treeBoots.Requisites[0].Id);
        Assert.Equal(1, treeBoots.Requisites[0].Value);
        Assert.Equal(7, treeBoots.Steps.Count);
        Assert.Equal(0x04, treeBoots.Steps[0].Value);
        Assert.Equal(0xA0, treeBoots.Steps[3].Value);
        Assert.Equal(0x80, treeBoots.Steps[4].Value);

        var fishingPole = sideQuests[2];
        Assert.Equal("fishingPole", fishingPole.Id);
        Assert.Single(fishingPole.Requisites);
        Assert.Equal(3, fishingPole.Steps.Count);
        Assert.Equal(3, fishingPole.Steps[2].Requisites.Count);
        Assert.Equal("bambooSpear", fishingPole.Steps[2].Requisites[0].Id);
        Assert.Equal(1, fishingPole.Steps[2].Requisites[0].Value);
        Assert.Equal("spiderWeb", fishingPole.Steps[2].Requisites[1].Id);
        Assert.Equal(1, fishingPole.Steps[2].Requisites[1].Value);
        Assert.Equal("redSnapper", fishingPole.Steps[2].Requisites[2].Id);
        Assert.Equal(0, fishingPole.Steps[2].Requisites[2].Value);
        Assert.Equal(1, fishingPole.Steps[2].Value);
    }

    [Fact]
    public void LoadLegendaryWeapons_ShouldIntegrateLegendaryWeaponAddressesAndReaderPipeline()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004B38E, 0x01)).Returns((byte)0x01);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004B38E, 0x02)).Returns((byte)0x02);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004B38E, 0x04)).Returns((byte)0x04);

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);

        var legendaryWeapons = questLoader.LoadLegendaryWeapons();

        Assert.NotNull(legendaryWeapons);
        Assert.Equal(3, legendaryWeapons.Count);

        var eternally = legendaryWeapons[0];
        Assert.Equal("eternally", eternally.Id);
        Assert.Empty(eternally.Requisites);
        Assert.Single(eternally.Steps);
        Assert.Equal(1, eternally.Steps[0].Number);
        Assert.Equal(0x01, eternally.Steps[0].Value);

        var invincible = legendaryWeapons[1];
        Assert.Equal("invincible", invincible.Id);
        Assert.Empty(invincible.Requisites);
        Assert.Single(invincible.Steps);
        Assert.Equal(1, invincible.Steps[0].Number);
        Assert.Equal(0x02, invincible.Steps[0].Value);

        var muramasa = legendaryWeapons[2];
        Assert.Equal("muramasa", muramasa.Id);
        Assert.Empty(muramasa.Requisites);
        Assert.Single(muramasa.Steps);
        Assert.Equal(1, muramasa.Steps[0].Number);
        Assert.Equal(0x04, muramasa.Steps[0].Value);
    }

    [Fact]
    public void LoadDriAgents_ShouldIntegrateDriAgentAddressesAndReaderPipeline()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004B38C, 0x02)).Returns((byte)0x02);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004B3B7, 0x08)).Returns((byte)0x08);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x00048DD2, null)).Returns((byte)1);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004A7E0, 0x08)).Returns((byte)0x08);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004B38C, 0x01)).Returns((byte)0x01);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004B3B7, 0x04)).Returns((byte)0x04);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x00048DB6, null)).Returns((byte)1);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004A028, 0x06)).Returns((byte)0x06);

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);

        var driAgents = questLoader.LoadDriAgents();

        Assert.NotNull(driAgents);
        Assert.Equal(2, driAgents.Count);

        var guilmon = driAgents[0];
        Assert.Equal("driAgentGuilmon", guilmon.Id);
        Assert.Empty(guilmon.Requisites);
        Assert.Equal(3, guilmon.Steps.Count);
        Assert.Equal(0x02, guilmon.Steps[0].Value);
        Assert.Equal(0x08, guilmon.Steps[1].Value);
        Assert.Single(guilmon.Steps[1].Requisites);
        Assert.Equal("guilmonDDNA", guilmon.Steps[1].Requisites[0].Id);
        Assert.Equal(1, guilmon.Steps[1].Requisites[0].Value);
        Assert.Equal(0x08, guilmon.Steps[2].Value);

        var agumon = driAgents[1];
        Assert.Equal("driAgentAgumon", agumon.Id);
        Assert.Empty(agumon.Requisites);
        Assert.Equal(3, agumon.Steps.Count);
        Assert.Equal(0x01, agumon.Steps[0].Value);
        Assert.Equal(0x04, agumon.Steps[1].Value);
        Assert.Single(agumon.Steps[1].Requisites);
        Assert.Equal("agumonDDNA", agumon.Steps[1].Requisites[0].Id);
        Assert.Equal(1, agumon.Steps[1].Requisites[0].Value);
        Assert.Equal(0x06, agumon.Steps[2].Value);
    }
}
