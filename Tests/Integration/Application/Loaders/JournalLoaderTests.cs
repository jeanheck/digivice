namespace Tests.Integration.Application.Loaders;

using Backend.Application.Loaders;
using Backend.Application.Loaders.Journals;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Journals;
using Backend.Memory.Readers.Journals.Quests;
using Moq;
using Xunit;

public class JournalLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void Load_ShouldIntegrateJournalAndQuestLoaderPipeline()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(It.IsAny<long>(), 1)).Returns([0]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B3B6, 1)).Returns([(byte)0x80]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048F42, 1)).Returns([(byte)1]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x04B3B0, 1)).Returns([(byte)0x04]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B38E, 1)).Returns([(byte)0x01]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B38C, 1)).Returns([(byte)0x02]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B3B7, 1)).Returns([(byte)0x00]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DD2, 1)).Returns([(byte)0]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004A7E0, 1)).Returns([(byte)0x00]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DB6, 1)).Returns([(byte)0]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004A028, 1)).Returns([(byte)0x00]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B38A, 1)).Returns([(byte)0x01]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B3DF, 1)).Returns([(byte)0x20]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B39A, 1)).Returns([(byte)0xFF]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B39B, 1)).Returns([(byte)0xFF]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B39C, 1)).Returns([(byte)0x01]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DB9, 1)).Returns([(byte)0x01]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DBA, 1)).Returns([(byte)0x01]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DBB, 1)).Returns([(byte)0x01]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B3E0, 1)).Returns([(byte)0x40]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B3E1, 1)).Returns([(byte)0x40]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004B3CA, 1)).Returns([(byte)0x20]);

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);
        var auctionReader = new AuctionReader(memoryReaderMock.Object);
        var auctionLoader = new AuctionLoader(addressesRepository, auctionReader);
        var npcReader = new NpcReader(memoryReaderMock.Object);
        var npcLoader = new NpcLoader(addressesRepository, npcReader);
        var journalLoader = new JournalLoader(questLoader, auctionLoader, npcLoader);

        var journalResource = journalLoader.Load();

        Assert.NotNull(journalResource);
        Assert.Equal("mainQuest", journalResource.MainQuest.Id);
        Assert.Equal(61, journalResource.MainQuest.Steps.Count);
        Assert.Equal(0x80, journalResource.MainQuest.Steps[0].Value);
        Assert.Equal(3, journalResource.SideQuests.Count);
        var folderBag = Assert.Single(journalResource.SideQuests, quest => quest.Id == "folderBag");
        var treeBoots = Assert.Single(journalResource.SideQuests, quest => quest.Id == "treeBoots");
        Assert.Single(journalResource.SideQuests, quest => quest.Id == "fishingPole");
        Assert.Equal(1, folderBag.Steps[0].Value);
        Assert.Equal(0x04, treeBoots.Steps[0].Value);
        Assert.Equal(5, journalResource.LegendaryWeapons.Count);
        var eternally = Assert.Single(journalResource.LegendaryWeapons, quest => quest.Id == "eternally");
        Assert.Single(journalResource.LegendaryWeapons, quest => quest.Id == "invincible");
        Assert.Single(journalResource.LegendaryWeapons, quest => quest.Id == "muramasa");
        Assert.Single(journalResource.LegendaryWeapons, quest => quest.Id == "superNova");
        Assert.Single(journalResource.LegendaryWeapons, quest => quest.Id == "punishment");
        Assert.Equal(0x01, eternally.Steps[0].Value);
        Assert.Equal(0, Assert.Single(journalResource.LegendaryWeapons, quest => quest.Id == "invincible").Steps[0].Value);
        Assert.Equal(0, Assert.Single(journalResource.LegendaryWeapons, quest => quest.Id == "muramasa").Steps[0].Value);
        Assert.Equal(0, Assert.Single(journalResource.LegendaryWeapons, quest => quest.Id == "superNova").Steps[0].Value);
        Assert.Equal(0, Assert.Single(journalResource.LegendaryWeapons, quest => quest.Id == "punishment").Steps[0].Value);
        Assert.Equal(8, journalResource.DriAgents.Count);
        var guilmon = Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentGuilmon");
        Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentAgumon");
        Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentVeemon");
        Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentKumamon");
        Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentMonmon");
        Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentKotemon");
        Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentRenamon");
        Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentPatamon");
        Assert.Equal(0x02, guilmon.Steps[0].Value);
        Assert.Equal(0, guilmon.Steps[1].Value);
        Assert.Equal(0, Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentAgumon").Steps[0].Value);
        Assert.Equal(0, Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentVeemon").Steps[0].Value);
        Assert.Equal(0, Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentKumamon").Steps[0].Value);
        Assert.Equal(0, Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentMonmon").Steps[0].Value);
        Assert.Equal(0, Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentKotemon").Steps[0].Value);
        Assert.Equal(0, Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentRenamon").Steps[0].Value);
        Assert.Equal(0, Assert.Single(journalResource.DriAgents, quest => quest.Id == "driAgentPatamon").Steps[0].Value);
        Assert.Equal(2, journalResource.DuelIsland.Count);
        Assert.Single(journalResource.DuelIsland, quest => quest.Id == "asukaTrophy");
        Assert.Single(journalResource.DuelIsland, quest => quest.Id == "sunTrophy");
        Assert.Equal(5, journalResource.Auctions.Count);
        Assert.Contains(journalResource.Auctions, auction => auction.Id == "divineBarrier" && auction.Value == 0x01);
        Assert.Contains(journalResource.Auctions, auction => auction.Id == "hazardShield" && auction.Value == 0x00);
        Assert.Equal(23, journalResource.Npcs.Count);
        var genji = Assert.Single(journalResource.Npcs, npc => npc.Id == "genji");
        Assert.Equal(0x20, genji.DigimonBattles.Single(battle => battle.Id == "first").Value);
        Assert.Equal(0x01, genji.DigimonBattles.Single(battle => battle.Id == "second").Value);
        Assert.Equal(0x02, Assert.Single(journalResource.Npcs, npc => npc.Id == "natsumi").DigimonBattles[0].Value);
        Assert.Equal(0x08, Assert.Single(journalResource.Npcs, npc => npc.Id == "catherine").DigimonBattles[0].Value);
        Assert.Equal(0x20, Assert.Single(journalResource.Npcs, npc => npc.Id == "robert").DigimonBattles[0].Value);
        Assert.Equal(0x02, Assert.Single(journalResource.Npcs, npc => npc.Id == "chris").DigimonBattles[0].Value);
        Assert.Equal(0x01, Assert.Single(journalResource.Npcs, npc => npc.Id == "tomomi").DigimonBattles[0].Value);
        Assert.Equal(0x04, Assert.Single(journalResource.Npcs, npc => npc.Id == "mitch").DigimonBattles[0].Value);
        Assert.Equal(0x80, Assert.Single(journalResource.Npcs, npc => npc.Id == "bob").DigimonBattles[0].Value);
        Assert.Equal(0x04, Assert.Single(journalResource.Npcs, npc => npc.Id == "andy").DigimonBattles[0].Value);
        Assert.Equal(0x08, Assert.Single(journalResource.Npcs, npc => npc.Id == "george").DigimonBattles[0].Value);
        Assert.Equal(0x10, Assert.Single(journalResource.Npcs, npc => npc.Id == "meiLin").DigimonBattles[0].Value);
        Assert.Equal(0x20, Assert.Single(journalResource.Npcs, npc => npc.Id == "jessica").DigimonBattles[0].Value);
        Assert.Equal(0x40, Assert.Single(journalResource.Npcs, npc => npc.Id == "gordon").DigimonBattles[0].Value);
        Assert.Equal(0x80, Assert.Single(journalResource.Npcs, npc => npc.Id == "alice").DigimonBattles[0].Value);
        Assert.Equal(0x01, Assert.Single(journalResource.Npcs, npc => npc.Id == "nakano").DigimonBattles[0].Value);
        Assert.Equal(0x01, Assert.Single(journalResource.Npcs, npc => npc.Id == "seiryuLeader").DigimonBattles[0].Value);
        Assert.Equal(0x40, Assert.Single(journalResource.Npcs, npc => npc.Id == "keith").DigimonBattles[0].Value);
        Assert.Equal(0x01, Assert.Single(journalResource.Npcs, npc => npc.Id == "suzakuLeader").DigimonBattles[0].Value);
        Assert.Equal(0x40, Assert.Single(journalResource.Npcs, npc => npc.Id == "fakeByakkoLeader").DigimonBattles[0].Value);
        Assert.Equal(0x01, Assert.Single(journalResource.Npcs, npc => npc.Id == "byakkoLeader").DigimonBattles[0].Value);
        Assert.Equal(0x20, Assert.Single(journalResource.Npcs, npc => npc.Id == "aoaAttacker").DigimonBattles[0].Value);
    }
}
