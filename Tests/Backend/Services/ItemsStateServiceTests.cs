using Backend.Interfaces;
using Backend.Models;
using Backend.Models.Addresses;
using Backend.Models.Resources;
using Backend.Services;
using Moq;
using Xunit;

namespace Tests.Backend.Services
{
    public class ItemsStateServiceTests
    {
        private readonly Mock<IGameDatabase> _mockDatabase;
        private readonly Mock<IGameReader> _mockReader;
        private readonly ItemsStateService _itemsService;

        public ItemsStateServiceTests()
        {
            _mockDatabase = new Mock<IGameDatabase>();
            _mockReader = new Mock<IGameReader>();
            _itemsService = new ItemsStateService(_mockDatabase.Object, _mockReader.Object);
        }

        [Fact]
        public void GetImportantItems_ShouldMapBooleanFlags_BasedOnReaderBytes()
        {
            var fakeAddresses = new ImportantItemsAddresses
            {
                FolderBag = new ItemAddress { Id = "1", Name = "Folder Bag", Address = "0A" },
                TreeBoots = new ItemAddress { Id = "2", Name = "Tree Boots", Address = "0B" },
                FishingPole = new ItemAddress { Id = "3", Name = "Fishing Pole", Address = "0C" },
                RedSnapper = new ItemAddress { Id = "4", Name = "Red Snapper", Address = "0D" }
            };

            _mockDatabase.Setup(db => db.GetImportantItemsAddresses()).Returns(fakeAddresses);

            _mockReader.Setup(r => r.ReadImportantItems(It.IsAny<ImportantItemsAddresses>()))
                       .Returns(new ImportantItemsResource
                       {
                           Folderbag = 1,
                           TreeBoots = 0,
                           FishingPole = 1,
                           RedSnapper = 0
                       });

            var result = _itemsService.GetImportantItems();

            Assert.True(result.FolderBag?.Has);
            Assert.False(result.TreeBoots?.Has);
            Assert.True(result.FishingPole?.Has);
            Assert.False(result.RedSnapper?.Has);
        }

        [Fact]
        public void GetConsumableItems_ShouldMapQuantity_BasedOnReaderBytes()
        {
            var fakeAddresses = new ConsumableItemsAddresses
            {
                PowerCharge = new ItemAddress { Id = "1", Name = "Power Charge", Address = "0x1" },
                SpiderWeb = new ItemAddress { Id = "2", Name = "Spider Web", Address = "0x2" },
                BambooSpear = new ItemAddress { Id = "3", Name = "Bamboo Spear", Address = "0x3" }
            };

            _mockDatabase.Setup(db => db.GetConsumableItemsAddresses()).Returns(fakeAddresses);

            _mockReader.Setup(r => r.ReadConsumableItems(It.IsAny<ConsumableItemsAddresses>()))
                       .Returns(new ConsumableItemsResource
                       {
                           PowerCharge = 5,
                           SpiderWeb = 99,
                           BambooSpear = 0
                       });

            var result = _itemsService.GetConsumableItems();

            Assert.Equal(5, result.PowerCharge?.Quantity);
            Assert.Equal(99, result.SpiderWeb?.Quantity);
            Assert.Equal(0, result.BambooSpear?.Quantity);
        }
    }
}
