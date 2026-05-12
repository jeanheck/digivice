using Backend.Services;
using Backend.Interfaces;
using Backend.Models.Addresses;
using Backend.Models.Quests;
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

        [Fact]
        public void ReadParty_ShouldReturnActiveDigimonIds()
        {
            var addresses = new PartyAddresses
            {
                PartySlot1 = "0x10",
                PartySlot2 = "0x20",
                PartySlot3 = "0x30",
                PartySlotStride = 4
            };

            _mockMemoryReader.Setup(m => m.ReadBytes(It.IsAny<long>(), 4))
                             .Returns((long address, int length) =>
                             {
                                 if (address == 0x10) return new byte[] { 5, 0, 0, 0 }; // ID 5
                                 if (address == 0x20) return new byte[] { 8, 0, 0, 0 }; // ID 8
                                 if (address == 0x30) return new byte[] { 0, 0, 0, 0 }; // ID 0 empty
                                 return Array.Empty<byte>();
                             });

            var reader = new GameReader(_mockMemoryReader.Object);
            var result = reader.ReadParty(addresses);

            Assert.Equal(3, result.DigimonIds.Count);
            Assert.Contains((byte)5, result.DigimonIds);
            Assert.Contains((byte)8, result.DigimonIds);
            Assert.Contains((byte)0, result.DigimonIds);
        }

        [Fact]
        public void ReadConsumableItems_ShouldMapResourceFully()
        {
            var addresses = new ConsumableItemsAddresses
            {
                PowerCharge = new ItemAddress { Id = "PowerCharge", Address = "0x40" },
                SpiderWeb = new ItemAddress { Id = "SpiderWeb", Address = "0x41" },
                BambooSpear = new ItemAddress { Id = "BambooSpear", Address = "0x42" }
            };

            _mockMemoryReader.Setup(m => m.ReadBytes(It.IsAny<long>(), 1))
                             .Returns((long address, int length) =>
                             {
                                 if (address == 0x40) return new byte[] { 10 }; // 10 charges
                                 if (address == 0x41) return new byte[] { 5 }; // 5 webs
                                 if (address == 0x42) return new byte[] { 0 }; // 0 spears
                                 return new byte[] { 0 };
                             });

            var reader = new GameReader(_mockMemoryReader.Object);
            var result = reader.ReadConsumableItems(addresses);

            Assert.Equal(10, result.PowerCharge);
            Assert.Equal(5, result.SpiderWeb);
            Assert.Equal(0, result.BambooSpear);
        }

        [Fact]
        public void ReadQuestSteps_ShouldMapDictionaryCorrectly()
        {
            var steps = new List<QuestStep>
            {
                new QuestStep { Number = 1, Address = "0x50" },
                new QuestStep { Number = 2, Address = "0x51" },
                new QuestStep { Number = 3, Address = "" } // Missing address
            };

            _mockMemoryReader.Setup(m => m.ReadBytes(It.IsAny<long>(), 1))
                             .Returns((long address, int length) =>
                             {
                                 if (address == 0x50) return new byte[] { 255 }; // completed byte
                                 if (address == 0x51) return new byte[] { 0 }; // not completed
                                 return new byte[] { 0 };
                             });

            var reader = new GameReader(_mockMemoryReader.Object);
            var result = reader.ReadQuestSteps(steps);

            Assert.Equal(2, result.Count);
            Assert.Equal(255, result[1]);
            Assert.Equal(0, result[2]);
            // Step 3 shouldn't be mapped
            Assert.False(result.ContainsKey(3));
        }
    }
}
