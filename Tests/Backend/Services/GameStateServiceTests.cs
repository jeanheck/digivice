using Backend.Interfaces;
using Backend.Models.Addresses;
using Backend.Models.Resources;
using Backend.Services;
using global::Backend.Models.Quests;
using Moq;

namespace Tests.Backend.Services
{
    public class GameStateServiceTests
    {
        [Fact]
        public void GetState_ShouldReturnState_WithAllPropertiesPopulated()
        {
            // Arrange
            var mockDb = new Mock<IGameDatabase>();
            var mockReader = new Mock<IGameReader>();
            var mockMemoryReader = new Mock<IMemoryReaderService>();

            // Setup basic mocks for sub-services
            // Player
            mockDb.Setup(db => db.GetPlayerAddresses()).Returns(new PlayerAddresses());
            mockReader.Setup(r => r.ReadPlayer(It.IsAny<PlayerAddresses>())).Returns(new PlayerResource
            {
                NameBytes = new byte[] { 0x17, 0x2C, 0x28, 0x35, 0x00 }, // Jean in Digimon World 3 custom encoding
                Bits = 1000,
                MapId = 20
            });

            // Party
            mockDb.Setup(db => db.GetPartyAddresses()).Returns(new PartyAddresses());
            var partyResource = new PartyResource();
            partyResource.ActiveDigimonIds.Add(1);
            mockReader.Setup(r => r.ReadParty(It.IsAny<PartyAddresses>())).Returns(partyResource);

            // Digimon
            mockDb.Setup(db => db.GetDigimonAddresses()).Returns(new DigimonAddresses());
            mockReader.Setup(r => r.ReadDigimonResource(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DigimonAddresses>())).Returns(new DigimonResource());

            // Items
            var importantMock = new ImportantItemsAddresses
            {
                FolderBag = new ItemAddress { Id = "FolderBag", Name = "Folder Bag" },
                TreeBoots = new ItemAddress { Id = "TreeBoots", Name = "Tree Boots" },
                FishingPole = new ItemAddress { Id = "FishingPole", Name = "Fishing Pole" },
                RedSnapper = new ItemAddress { Id = "RedSnapper", Name = "Red Snapper" }
            };
            mockDb.Setup(db => db.GetImportantItemsAddresses()).Returns(importantMock);
            mockReader.Setup(r => r.ReadImportantItems(It.IsAny<ImportantItemsAddresses>())).Returns(new ImportantItemsResource { FishingPole = 1 });

            // Consumables
            var consMock = new ConsumableItemsAddresses
            {
                PowerCharge = new ItemAddress { Id = "PowerCharge", Name = "Power Charge" },
                SpiderWeb = new ItemAddress { Id = "SpiderWeb", Name = "Spider Web" },
                BambooSpear = new ItemAddress { Id = "BambooSpear", Name = "Bamboo Spear" }
            };
            mockDb.Setup(db => db.GetConsumableItemsAddresses()).Returns(consMock);
            mockReader.Setup(r => r.ReadConsumableItems(It.IsAny<ConsumableItemsAddresses>())).Returns(new ConsumableItemsResource());

            // Journal
            mockDb.Setup(db => db.GetMainQuest()).Returns(new MainQuest { Steps = new List<QuestStep>() });
            mockDb.Setup(db => db.GetSideQuest(It.IsAny<string>())).Returns(new SideQuest { Steps = new List<QuestStep>() });
            mockReader.Setup(r => r.ReadQuestSteps(It.IsAny<List<QuestStep>>())).Returns(new Dictionary<int, byte>());

            var playerService = new PlayerStateService(mockDb.Object, mockReader.Object);
            var digiEvoService = new DigievolutionStateService();
            var digimonService = new DigimonStateService(mockDb.Object, mockReader.Object, digiEvoService);
            var partyService = new PartyStateService(mockDb.Object, mockReader.Object, digimonService);
            var itemsService = new ItemsStateService(mockDb.Object, mockReader.Object);
            var journalService = new JournalStateService(mockDb.Object, mockReader.Object, itemsService);

            var gameStateService = new GameStateService(mockMemoryReader.Object, playerService, partyService, itemsService, journalService);

            // Act
            var state = gameStateService.GetState();

            // Assert
            Assert.NotNull(state);
            Assert.NotNull(state.Player);
            Assert.Equal("Jean", state.Player.Name);
            Assert.Equal(1000, state.Player.Bits);

            Assert.NotNull(state.Party);
            Assert.NotNull(state.ImportantItems);
            Assert.True(state.ImportantItems.FishingPole?.Has ?? false);

            Assert.NotNull(state.Journal);
        }
    }
}
