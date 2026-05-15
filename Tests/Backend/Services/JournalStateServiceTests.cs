using Backend.Interfaces;
using Backend.Models.Quests;
using Backend.Addresses;
using Backend.Services;
using Moq;

namespace Tests.Backend.Services
{
    public class JournalStateServiceTests
    {
        private readonly Mock<IGameDatabase> _mockDatabase;
        private readonly Mock<IGameReader> _mockReader;
        private readonly JournalStateService _journalService;

        public JournalStateServiceTests()
        {
            _mockDatabase = new Mock<IGameDatabase>();
            _mockReader = new Mock<IGameReader>();

            _journalService = new JournalStateService(_mockDatabase.Object, _mockReader.Object);
        }

        [Fact]
        public void GetJournal_ShouldMapQuests_BasedOnBitmask()
        {
            var mainQuestSteps = new List<QuestStep>
            {
                new QuestStep { Number = 1, BitMask = "0x02", Address = 0x10 }
            };

            var mainQuest = new MainQuest { Id = "Main", Steps = mainQuestSteps };

            _mockDatabase.Setup(db => db.GetMainQuest()).Returns(mainQuest);
            _mockDatabase.Setup(db => db.GetAllSideQuests()).Returns(new List<SideQuest>());

            // Simulates memory byte reading for Step 1
            _mockReader.Setup(r => r.ReadQuestSteps(It.IsAny<Quest>()))
                       .Returns(new Dictionary<int, byte> { { 1, 0x02 } });

            var journal = _journalService.GetJournal();

            Assert.True(journal.MainQuest?.Steps[0].IsCompleted);
        }
    }
}
