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
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B3B6, 0x80)).Returns((byte)0x80);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x00048F42, null)).Returns((byte)1);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x00048F42, 0)).Returns((byte)1);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x04B3B0, 0x04)).Returns((byte)0x04);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B38E, 0x01)).Returns((byte)0x01);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B38E, 0x02)).Returns((byte)0x00);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B38E, 0x04)).Returns((byte)0x00);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B38C, 0x02)).Returns((byte)0x02);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B3B7, 0x08)).Returns((byte)0x00);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x00048DD2, null)).Returns((byte)0);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004A7E0, 0x08)).Returns((byte)0x00);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B38C, 0x01)).Returns((byte)0x00);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B3B7, 0x04)).Returns((byte)0x00);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x00048DB6, null)).Returns((byte)0);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004A028, 0x06)).Returns((byte)0x00);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B38A, 0x01)).Returns((byte)0x01);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B38A, 0x02)).Returns((byte)0x00);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B38A, 0x04)).Returns((byte)0x00);
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x0004B38A, 0x08)).Returns((byte)0x00);

        var requisiteReader = new RequisiteReader(memoryReaderMock.Object);
        var stepReader = new StepReader(memoryReaderMock.Object, requisiteReader);
        var questReader = new QuestReader(requisiteReader, stepReader);
        var questLoader = new QuestLoader(addressesRepository, questReader);
        var auctionReader = new AuctionReader(memoryReaderMock.Object);
        var auctionLoader = new AuctionLoader(addressesRepository, auctionReader);
        var journalLoader = new JournalLoader(questLoader, auctionLoader);

        var journalResource = journalLoader.Load();

        Assert.NotNull(journalResource);
        Assert.Equal("MainQuest", journalResource.MainQuest.Id);
        Assert.Equal(43, journalResource.MainQuest.Steps.Count);
        Assert.Equal(0x80, journalResource.MainQuest.Steps[0].Value);
        Assert.Equal(3, journalResource.SideQuests.Count);
        Assert.Equal("FolderBag", journalResource.SideQuests[0].Id);
        Assert.Equal("TreeBoots", journalResource.SideQuests[1].Id);
        Assert.Equal("FishingPole", journalResource.SideQuests[2].Id);
        Assert.Equal(1, journalResource.SideQuests[0].Steps[0].Value);
        Assert.Equal(0x04, journalResource.SideQuests[1].Steps[0].Value);
        Assert.Equal(3, journalResource.LegendaryWeapons.Count);
        Assert.Equal("eternally", journalResource.LegendaryWeapons[0].Id);
        Assert.Equal("invincible", journalResource.LegendaryWeapons[1].Id);
        Assert.Equal("muramasa", journalResource.LegendaryWeapons[2].Id);
        Assert.Equal(0x01, journalResource.LegendaryWeapons[0].Steps[0].Value);
        Assert.Equal(0, journalResource.LegendaryWeapons[1].Steps[0].Value);
        Assert.Equal(0, journalResource.LegendaryWeapons[2].Steps[0].Value);
        Assert.Equal(2, journalResource.DriAgents.Count);
        Assert.Equal("driAgentGuilmon", journalResource.DriAgents[0].Id);
        Assert.Equal("driAgentAgumon", journalResource.DriAgents[1].Id);
        Assert.Equal(0x02, journalResource.DriAgents[0].Steps[0].Value);
        Assert.Equal(0, journalResource.DriAgents[0].Steps[1].Value);
        Assert.Equal(0, journalResource.DriAgents[1].Steps[0].Value);
        Assert.Equal(4, journalResource.Auctions.Count);
        Assert.Contains(journalResource.Auctions, auction => auction.Id == "divineBarrier" && auction.Value == 0x01);
        Assert.Contains(journalResource.Auctions, auction => auction.Id == "hazardShield" && auction.Value == 0x00);
    }
}
