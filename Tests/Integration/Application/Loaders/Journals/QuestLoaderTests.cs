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
        memoryReaderMock.Setup(m => m.ReadBytes(It.IsAny<long>(), 1)).Returns([0]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B3B6, 1)).Returns([(byte)0x80]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B3B7, 1)).Returns([(byte)0x01]);

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);

        var mainQuest = questLoader.LoadMainQuest();

        Assert.NotNull(mainQuest);
        Assert.Equal("mainQuest", mainQuest.Id);
        Assert.Empty(mainQuest.Requisites);
        Assert.Equal(61, mainQuest.Steps.Count);
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
        memoryReaderMock.Setup(m => m.ReadBytes(It.IsAny<long>(), 1)).Returns([0]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048F42, 1)).Returns([(byte)1]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x04B3B0, 1)).Returns([(byte)0xA4]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x048E57, 1)).Returns([(byte)1]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x048E09, 1)).Returns([(byte)1]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DB7, 1)).Returns([(byte)0]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x048DB5, 1)).Returns([(byte)1]);

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);

        var sideQuests = questLoader.LoadSideQuests();

        Assert.NotNull(sideQuests);
        Assert.Equal(3, sideQuests.Count);

        var folderBag = Assert.Single(sideQuests, quest => quest.Id == "folderBag");
        Assert.Empty(folderBag.Requisites);
        Assert.Single(folderBag.Steps);
        Assert.Equal(1, folderBag.Steps[0].Value);

        var treeBoots = Assert.Single(sideQuests, quest => quest.Id == "treeBoots");
        Assert.Single(treeBoots.Requisites);
        Assert.Equal("FolderBag", treeBoots.Requisites[0].Id);
        Assert.Equal(1, treeBoots.Requisites[0].Value);
        Assert.Equal(7, treeBoots.Steps.Count);
        Assert.Equal(0x04, treeBoots.Steps[0].Value);
        Assert.Equal(0xA0, treeBoots.Steps[3].Value);
        Assert.Equal(0x80, treeBoots.Steps[4].Value);

        var fishingPole = Assert.Single(sideQuests, quest => quest.Id == "fishingPole");
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
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(It.IsAny<long>(), 1)).Returns([0]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B38E, 1)).Returns([(byte)0x1F]);

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);

        var legendaryWeapons = questLoader.LoadLegendaryWeapons();

        Assert.NotNull(legendaryWeapons);
        Assert.Equal(5, legendaryWeapons.Count);

        var eternally = Assert.Single(legendaryWeapons, quest => quest.Id == "eternally");
        Assert.Empty(eternally.Requisites);
        Assert.Single(eternally.Steps);
        Assert.Equal(1, eternally.Steps[0].Number);
        Assert.Equal(0x01, eternally.Steps[0].Value);

        var invincible = Assert.Single(legendaryWeapons, quest => quest.Id == "invincible");
        Assert.Single(invincible.Requisites);
        Assert.Equal("seiryuLeader", invincible.Requisites[0].Id);
        Assert.Equal(0, invincible.Requisites[0].Value);
        Assert.Single(invincible.Steps);
        Assert.Equal(1, invincible.Steps[0].Number);
        Assert.Equal(0x02, invincible.Steps[0].Value);

        var muramasa = Assert.Single(legendaryWeapons, quest => quest.Id == "muramasa");
        Assert.Single(muramasa.Requisites);
        Assert.Equal("smellHerb", muramasa.Requisites[0].Id);
        Assert.Equal(0, muramasa.Requisites[0].Value);
        Assert.Single(muramasa.Steps);
        Assert.Equal(1, muramasa.Steps[0].Number);
        Assert.Equal(0x04, muramasa.Steps[0].Value);

        var superNova = Assert.Single(legendaryWeapons, quest => quest.Id == "superNova");
        Assert.Single(superNova.Requisites);
        Assert.Equal("submarimon", superNova.Requisites[0].Id);
        Assert.Equal(0, superNova.Requisites[0].Value);
        Assert.Single(superNova.Steps);
        Assert.Equal(1, superNova.Steps[0].Number);
        Assert.Equal(0x08, superNova.Steps[0].Value);

        var punishment = Assert.Single(legendaryWeapons, quest => quest.Id == "punishment");
        Assert.Single(punishment.Requisites);
        Assert.Equal("numemon", punishment.Requisites[0].Id);
        Assert.Equal(0, punishment.Requisites[0].Value);
        Assert.Single(punishment.Steps);
        Assert.Equal(1, punishment.Steps[0].Number);
        Assert.Equal(0x10, punishment.Steps[0].Value);
    }

    [Fact]
    public void LoadDriAgents_ShouldIntegrateDriAgentAddressesAndReaderPipeline()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(It.IsAny<long>(), 1)).Returns([0]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B38C, 1)).Returns([(byte)0xFF]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B3B7, 1)).Returns([(byte)0xFC]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B3B8, 1)).Returns([(byte)0x03]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048DD2, 1)).Returns([(byte)1]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004A7E0, 1)).Returns([(byte)0x08]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048DB6, 1)).Returns([(byte)1]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004A028, 1)).Returns([(byte)0x06]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048DD3, 1)).Returns([(byte)1]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004A404, 1)).Returns([(byte)0x07]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048F3B, 1)).Returns([(byte)1]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00049870, 1)).Returns([(byte)0x04]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048F18, 1)).Returns([(byte)1]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00049C4C, 1)).Returns([(byte)0x05]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048DC3, 1)).Returns([(byte)1]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00049494, 1)).Returns([(byte)0x03]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048DD6, 1)).Returns([(byte)1]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004ABBC, 1)).Returns([(byte)0x09]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048DD7, 1)).Returns([(byte)1]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004AF98, 1)).Returns([(byte)0x0A]);

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);

        var driAgents = questLoader.LoadDriAgents();

        Assert.NotNull(driAgents);
        Assert.Equal(8, driAgents.Count);

        var guilmon = Assert.Single(driAgents, quest => quest.Id == "driAgentGuilmon");
        Assert.Empty(guilmon.Requisites);
        Assert.Equal(3, guilmon.Steps.Count);
        Assert.Equal(0x02, guilmon.Steps[0].Value);
        Assert.Equal(0x08, guilmon.Steps[1].Value);
        Assert.Equal(0x08, guilmon.Steps[2].Value);
        Assert.Single(guilmon.Steps[2].Requisites);
        Assert.Equal("guilmonDDNA", guilmon.Steps[2].Requisites[0].Id);
        Assert.Equal(1, guilmon.Steps[2].Requisites[0].Value);

        var agumon = Assert.Single(driAgents, quest => quest.Id == "driAgentAgumon");
        Assert.Empty(agumon.Requisites);
        Assert.Equal(3, agumon.Steps.Count);
        Assert.Equal(0x01, agumon.Steps[0].Value);
        Assert.Equal(0x04, agumon.Steps[1].Value);
        Assert.Equal(0x06, agumon.Steps[2].Value);
        Assert.Single(agumon.Steps[2].Requisites);
        Assert.Equal("agumonDDNA", agumon.Steps[2].Requisites[0].Id);
        Assert.Equal(1, agumon.Steps[2].Requisites[0].Value);

        var veemon = Assert.Single(driAgents, quest => quest.Id == "driAgentVeemon");
        Assert.Single(veemon.Requisites);
        Assert.Equal("byakkoLeader", veemon.Requisites[0].Id);
        Assert.Equal(0, veemon.Requisites[0].Value);
        Assert.Equal(3, veemon.Steps.Count);
        Assert.Equal(0x80, veemon.Steps[0].Value);
        Assert.Equal(0x02, veemon.Steps[1].Value);
        Assert.Equal(0x07, veemon.Steps[2].Value);
        Assert.Single(veemon.Steps[2].Requisites);
        Assert.Equal("veemonDDNA", veemon.Steps[2].Requisites[0].Id);
        Assert.Equal(1, veemon.Steps[2].Requisites[0].Value);

        var kumamon = Assert.Single(driAgents, quest => quest.Id == "driAgentKumamon");
        Assert.Single(kumamon.Requisites);
        Assert.Equal("submarimon", kumamon.Requisites[0].Id);
        Assert.Equal(0, kumamon.Requisites[0].Value);
        Assert.Equal(3, kumamon.Steps.Count);
        Assert.Equal(0x20, kumamon.Steps[0].Value);
        Assert.Equal(0x01, kumamon.Steps[1].Value);
        Assert.Equal(0x04, kumamon.Steps[2].Value);
        Assert.Single(kumamon.Steps[2].Requisites);
        Assert.Equal("kumamonDDNA", kumamon.Steps[2].Requisites[0].Id);
        Assert.Equal(1, kumamon.Steps[2].Requisites[0].Value);

        var monmon = Assert.Single(driAgents, quest => quest.Id == "driAgentMonmon");
        Assert.Single(monmon.Requisites);
        Assert.Equal("submarimon", monmon.Requisites[0].Id);
        Assert.Equal(0, monmon.Requisites[0].Value);
        Assert.Equal(3, monmon.Steps.Count);
        Assert.Equal(0x40, monmon.Steps[0].Value);
        Assert.Equal(0x80, monmon.Steps[1].Value);
        Assert.Equal(0x05, monmon.Steps[2].Value);
        Assert.Single(monmon.Steps[2].Requisites);
        Assert.Equal("monmonDDNA", monmon.Steps[2].Requisites[0].Id);
        Assert.Equal(1, monmon.Steps[2].Requisites[0].Value);

        var kotemon = Assert.Single(driAgents, quest => quest.Id == "driAgentKotemon");
        Assert.Single(kotemon.Requisites);
        Assert.Equal("submarimon", kotemon.Requisites[0].Id);
        Assert.Equal(0, kotemon.Requisites[0].Value);
        Assert.Equal(3, kotemon.Steps.Count);
        Assert.Equal(0x10, kotemon.Steps[0].Value);
        Assert.Equal(0x40, kotemon.Steps[1].Value);
        Assert.Equal(0x03, kotemon.Steps[2].Value);
        Assert.Single(kotemon.Steps[2].Requisites);
        Assert.Equal("kotemonDDNA", kotemon.Steps[2].Requisites[0].Id);
        Assert.Equal(1, kotemon.Steps[2].Requisites[0].Value);

        var renamon = Assert.Single(driAgents, quest => quest.Id == "driAgentRenamon");
        Assert.Single(renamon.Requisites);
        Assert.Equal("submarimon", renamon.Requisites[0].Id);
        Assert.Equal(0, renamon.Requisites[0].Value);
        Assert.Equal(3, renamon.Steps.Count);
        Assert.Equal(0x08, renamon.Steps[0].Value);
        Assert.Equal(0x20, renamon.Steps[1].Value);
        Assert.Equal(0x09, renamon.Steps[2].Value);
        Assert.Single(renamon.Steps[2].Requisites);
        Assert.Equal("renamonDDNA", renamon.Steps[2].Requisites[0].Id);
        Assert.Equal(1, renamon.Steps[2].Requisites[0].Value);

        var patamon = Assert.Single(driAgents, quest => quest.Id == "driAgentPatamon");
        Assert.Single(patamon.Requisites);
        Assert.Equal("submarimon", patamon.Requisites[0].Id);
        Assert.Equal(0, patamon.Requisites[0].Value);
        Assert.Equal(3, patamon.Steps.Count);
        Assert.Equal(0x04, patamon.Steps[0].Value);
        Assert.Equal(0x10, patamon.Steps[1].Value);
        Assert.Equal(0x0A, patamon.Steps[2].Value);
        Assert.Single(patamon.Steps[2].Requisites);
        Assert.Equal("patamonDDNA", patamon.Steps[2].Requisites[0].Id);
        Assert.Equal(1, patamon.Steps[2].Requisites[0].Value);
    }

    [Fact]
    public void LoadDuelIsland_ShouldIntegrateAsukaTrophyAddressesAndReaderPipeline()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(It.IsAny<long>(), 1)).Returns([0]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B3B2, 1)).Returns([(byte)0xCF]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B3B3, 1)).Returns([(byte)0x0F]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048DC2, 1)).Returns([(byte)0x01]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048DC4, 1)).Returns([(byte)0x01]);

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);

        var duelIsland = questLoader.LoadDuelIsland();

        Assert.NotNull(duelIsland);
        Assert.Equal(2, duelIsland.Count);

        var asukaTrophy = Assert.Single(duelIsland, quest => quest.Id == "asukaTrophy");
        Assert.Empty(asukaTrophy.Requisites);
        Assert.Equal(6, asukaTrophy.Steps.Count);
        Assert.Equal(0x80, asukaTrophy.Steps[0].Value);
        Assert.Equal(0x01, asukaTrophy.Steps[1].Value);
        Assert.Equal(0x02, asukaTrophy.Steps[2].Value);
        Assert.Equal(0x04, asukaTrophy.Steps[3].Value);
        Assert.Equal(0x08, asukaTrophy.Steps[4].Value);
        Assert.Equal(1, asukaTrophy.Steps[5].Value);

        var sunTrophy = Assert.Single(duelIsland, quest => quest.Id == "sunTrophy");
        Assert.Single(sunTrophy.Requisites);
        Assert.Equal("asukaTrophy", sunTrophy.Requisites[0].Id);
        Assert.Equal(1, sunTrophy.Requisites[0].Value);
        Assert.Equal(6, sunTrophy.Steps.Count);
        Assert.Equal(0x80, sunTrophy.Steps[0].Value);
        Assert.Equal(0x01, sunTrophy.Steps[1].Value);
        Assert.Equal(0x02, sunTrophy.Steps[2].Value);
        Assert.Equal(0x04, sunTrophy.Steps[3].Value);
        Assert.Equal(0x08, sunTrophy.Steps[4].Value);
        Assert.Equal(1, sunTrophy.Steps[5].Value);
    }
}
