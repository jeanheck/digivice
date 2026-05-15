using Backend.Interfaces;
using Backend.Resources;
using Backend.Addresses;
using Backend.Services;
using Backend.Models.Quests;
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

            // Setup basic mocks for sub-services
            // Player
            mockDb.Setup(db => db.GetPlayerAddresses()).Returns(new PlayerAddresses());
            mockReader.Setup(r => r.ReadPlayer(It.IsAny<PlayerAddresses>())).Returns(new PlayerResource
            {
                NameInBytes = new byte[] { 0x17, 0x2C, 0x28, 0x35, 0x00 }, // Jean
                Bits = 1000,
                MapId = 20
            });

            // Party
            mockDb.Setup(db => db.GetPartyAddresses()).Returns(new PartyAddresses());
            var partyResource = new PartyResource();
            partyResource.DigimonIds.Add(1);
            mockReader.Setup(r => r.ReadParty(It.IsAny<PartyAddresses>())).Returns(partyResource);

            // Digimon
            mockDb.Setup(db => db.GetDigimonAddresses()).Returns(new DigimonAddresses());
            mockReader.Setup(r => r.ReadDigimon(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DigimonAddresses>())).Returns(new DigimonResource());

            // Journal
            mockDb.Setup(db => db.GetMainQuest()).Returns(new MainQuest { Steps = new List<QuestStep>() });
            mockDb.Setup(db => db.GetAllSideQuests()).Returns(new List<SideQuest>());
            mockReader.Setup(r => r.ReadQuestSteps(It.IsAny<Quest>())).Returns(new Dictionary<int, byte>());

            var playerService = new PlayerStateService(mockDb.Object, mockReader.Object);
            var digiEvoService = new DigievolutionStateService();
            var digimonService = new DigimonStateService(mockDb.Object, mockReader.Object, digiEvoService);
            var partyService = new PartyStateService(mockDb.Object, mockReader.Object, digimonService);
            var journalService = new JournalStateService(mockDb.Object, mockReader.Object);

            var gameStateService = new GameStateService(playerService, partyService, journalService);

            // Act
            var state = gameStateService.GetState();

            // Assert
            Assert.NotNull(state);
            Assert.NotNull(state.Player);
            Assert.Equal("Jean", state.Player.Name);
            Assert.Equal(1000, state.Player.Bits);

            Assert.NotNull(state.Party);
            Assert.NotNull(state.Journal);
        }
    }
}
