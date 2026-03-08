using Backend.Interfaces;
using Backend.Models.Quests;
using Backend.Models.Addresses;
using Backend.Models.Resources;
using Backend.Services;
using Moq;

namespace Tests.Backend.Services
{
    public class JournalStateServiceTests
    {
        private readonly Mock<IGameDatabase> _mockDatabase;
        private readonly Mock<IGameReader> _mockReader;
        private readonly ItemsStateService _itemsService;
        private readonly JournalStateService _journalService;

        public JournalStateServiceTests()
        {
            _mockDatabase = new Mock<IGameDatabase>();
            _mockReader = new Mock<IGameReader>();

            _itemsService = new ItemsStateService(_mockDatabase.Object, _mockReader.Object);
            _journalService = new JournalStateService(_mockDatabase.Object, _mockReader.Object, _itemsService);
        }

        [Fact]
        public void GetJournal_ShouldMapQuests_BasedOnBitmask()
        {
            // Populate Items Fake addresses so Journals can cross-reference properly without exceptions
            var fakeImpAdd = new ImportantItemsAddresses
            {
                FolderBag = new ItemAddress { Id = "1", Name = "Folder Bag" },
                TreeBoots = new ItemAddress { Id = "2", Name = "Tree Boots" },
                FishingPole = new ItemAddress { Id = "3", Name = "Fishing Pole" },
                RedSnapper = new ItemAddress { Id = "4", Name = "Red Snapper" }
            };
            var fakeConsAdd = new ConsumableItemsAddresses
            {
                PowerCharge = new ItemAddress { Id = "1", Name = "P Charge" },
                SpiderWeb = new ItemAddress { Id = "2", Name = "Spider Web" },
                BambooSpear = new ItemAddress { Id = "3", Name = "B Spear" }
            };
            _mockDatabase.Setup(db => db.GetImportantItemsAddresses()).Returns(fakeImpAdd);
            _mockDatabase.Setup(db => db.GetConsumableItemsAddresses()).Returns(fakeConsAdd);

            _mockReader.Setup(r => r.ReadImportantItems(It.IsAny<ImportantItemsAddresses>())).Returns(new ImportantItemsResource());
            _mockReader.Setup(r => r.ReadConsumableItems(It.IsAny<ConsumableItemsAddresses>())).Returns(new ConsumableItemsResource());


            var mainQuestSteps = new List<QuestStep>
            {
                new QuestStep { Number = 1, BitMask = "0x02", Address = "0x10" }
            };

            _mockDatabase.Setup(db => db.GetMainQuest()).Returns(new MainQuest { Steps = mainQuestSteps });
            _mockDatabase.Setup(db => db.GetSideQuest(It.IsAny<string>())).Returns(new SideQuest { Steps = new List<QuestStep>() });

            // Simulates memory byte reading for Step 1
            // Sent bitmask "0x02" means second bit must be active (which 0x02 == 0000 0010)
            _mockReader.Setup(r => r.ReadQuestSteps(It.IsAny<List<QuestStep>>()))
                       .Returns(new Dictionary<int, byte> { { 1, 0x02 } }); // Completed

            var journal = _journalService.GetJournal();

            Assert.True(journal.MainQuest?.Steps[0].IsCompleted);
        }
    }
}
