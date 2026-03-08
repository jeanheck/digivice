using Backend.Services;
using Backend.Interfaces;
using Backend.Models.Addresses;
using Moq;

namespace Tests.Backend.Services
{
    public class GameReaderTests
    {
        private readonly Mock<IMemoryReaderService> _mockMemoryReader;

        public GameReaderTests()
        {
            _mockMemoryReader = new Mock<IMemoryReaderService>();

            // Default safe connection response
            _mockMemoryReader.Setup(m => m.TryConnect()).Returns(true);
            _mockMemoryReader.Setup(m => m.IsConnected).Returns(true);
        }

        [Fact]
        public void ReadImportantItems_ShouldMapResourceFully()
        {
            var addresses = new ImportantItemsAddresses
            {
                FolderBag = new ItemAddress { Id = "FolderBag", Address = "0x20", Name = "A" },
                TreeBoots = new ItemAddress { Id = "TreeBoots", Address = "0x30", Name = "B" },
                FishingPole = new ItemAddress { Id = "FishingPole", Address = "0x00", Name = "C" },
                RedSnapper = new ItemAddress { Id = "RedSnapper", Address = "0x00", Name = "D" }
            };

            // Setup the specific mocked bytes returning for these addresses
            _mockMemoryReader.Setup(m => m.ReadBytes(It.IsAny<long>(), 1))
                             .Returns((long address, int length) =>
                             {
                                 if (address == 0x20) return new byte[] { 1 };
                                 if (address == 0x30) return new byte[] { 0 };
                                 return new byte[] { 0 };
                             });

            var reader = new GameReader(_mockMemoryReader.Object);
            var result = reader.ReadImportantItems(addresses);

            Assert.Equal(1, result.Folderbag);
            Assert.Equal(0, result.TreeBoots);
        }

        [Fact]
        public void ReadPlayerBits_ShouldReturnValidInt()
        {
            _mockMemoryReader.Setup(m => m.ReadInt32(It.IsAny<long>())).Returns(15000);

            var addresses = new PlayerAddresses { Bits = "0x50" };
            var reader = new GameReader(_mockMemoryReader.Object);

            var result = reader.ReadPlayer(addresses);

            Assert.Equal(15000, result.Bits);
        }
    }
}
