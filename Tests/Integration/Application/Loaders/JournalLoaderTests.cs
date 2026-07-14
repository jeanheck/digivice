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

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);
        var auctionReader = new AuctionReader(memoryReaderMock.Object);
        var auctionLoader = new AuctionLoader(addressesRepository, auctionReader);
        var journalLoader = new JournalLoader(questLoader, auctionLoader);

        var journalResource = journalLoader.Load();

        Assert.NotNull(journalResource);
        Assert.Equal("mainQuest", journalResource.MainQuest.Id);
        Assert.Equal(61, journalResource.MainQuest.Steps.Count);
        Assert.Equal(0x80, journalResource.MainQuest.Steps[0].Value);
        Assert.Equal(3, journalResource.SideQuests.Count);
        Assert.Equal("folderBag", journalResource.SideQuests[0].Id);
        Assert.Equal("treeBoots", journalResource.SideQuests[1].Id);
        Assert.Equal("fishingPole", journalResource.SideQuests[2].Id);
        Assert.Equal(1, journalResource.SideQuests[0].Steps[0].Value);
        Assert.Equal(0x04, journalResource.SideQuests[1].Steps[0].Value);
        Assert.Equal(5, journalResource.LegendaryWeapons.Count);
        Assert.Equal("eternally", journalResource.LegendaryWeapons[0].Id);
        Assert.Equal("invincible", journalResource.LegendaryWeapons[1].Id);
        Assert.Equal("muramasa", journalResource.LegendaryWeapons[2].Id);
        Assert.Equal("superNova", journalResource.LegendaryWeapons[3].Id);
        Assert.Equal("punishment", journalResource.LegendaryWeapons[4].Id);
        Assert.Equal(0x01, journalResource.LegendaryWeapons[0].Steps[0].Value);
        Assert.Equal(0, journalResource.LegendaryWeapons[1].Steps[0].Value);
        Assert.Equal(0, journalResource.LegendaryWeapons[2].Steps[0].Value);
        Assert.Equal(0, journalResource.LegendaryWeapons[3].Steps[0].Value);
        Assert.Equal(0, journalResource.LegendaryWeapons[4].Steps[0].Value);
        Assert.Equal(7, journalResource.DriAgents.Count);
        Assert.Equal("driAgentGuilmon", journalResource.DriAgents[0].Id);
        Assert.Equal("driAgentAgumon", journalResource.DriAgents[1].Id);
        Assert.Equal("driAgentVeemon", journalResource.DriAgents[2].Id);
        Assert.Equal("driAgentKumamon", journalResource.DriAgents[3].Id);
        Assert.Equal("driAgentMonmon", journalResource.DriAgents[4].Id);
        Assert.Equal("driAgentKotemon", journalResource.DriAgents[5].Id);
        Assert.Equal("driAgentRenamon", journalResource.DriAgents[6].Id);
        Assert.Equal(0x02, journalResource.DriAgents[0].Steps[0].Value);
        Assert.Equal(0, journalResource.DriAgents[0].Steps[1].Value);
        Assert.Equal(0, journalResource.DriAgents[1].Steps[0].Value);
        Assert.Equal(0, journalResource.DriAgents[2].Steps[0].Value);
        Assert.Equal(0, journalResource.DriAgents[3].Steps[0].Value);
        Assert.Equal(0, journalResource.DriAgents[4].Steps[0].Value);
        Assert.Equal(0, journalResource.DriAgents[5].Steps[0].Value);
        Assert.Equal(0, journalResource.DriAgents[6].Steps[0].Value);
        Assert.Equal(5, journalResource.Auctions.Count);
        Assert.Contains(journalResource.Auctions, auction => auction.Id == "divineBarrier" && auction.Value == 0x01);
        Assert.Contains(journalResource.Auctions, auction => auction.Id == "hazardShield" && auction.Value == 0x00);
    }
}
