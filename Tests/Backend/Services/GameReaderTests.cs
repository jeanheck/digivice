using Backend.Services;
using Backend.Interfaces;
using Backend.Addresses;
using Backend.Models.Quests;
using Backend.Resources;
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
        public void ReadPlayerBits_ShouldReturnValidInt()
        {
            _mockMemoryReader.Setup(m => m.ReadInt32(It.IsAny<long>())).Returns(15000);

            var addresses = new PlayerAddresses { Bits = 0x50 };
            var reader = new GameReader(_mockMemoryReader.Object);

            var result = reader.ReadPlayer(addresses);

            Assert.Equal(15000, result.Bits);
        }

        [Fact]
        public void ReadParty_ShouldReturnActiveDigimonIds()
        {
            var addresses = new PartyAddresses
            {
                PartySlot1 = 0x10,
                PartySlot2 = 0x20,
                PartySlot3 = 0x30,
                BytesPerSlot = 4
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
        public void ReadQuestSteps_ShouldMapDictionaryAndRequisites()
        {
            var quest = new MainQuest
            {
                Requisites = new List<Requisite> 
                { 
                    new Requisite { Id = "Req1", Address = 0x100 } 
                },
                Steps = new List<QuestStep>
                {
                    new QuestStep { Number = 1, Address = 0x50 },
                    new QuestStep { Number = 2, Address = 0x51 }
                }
            };

            _mockMemoryReader.Setup(m => m.ReadByteSafe(It.IsAny<long>()))
                             .Returns((long address) =>
                             {
                                 if (address == 0x100) return 1; // Requisite done
                                 if (address == 0x50) return 255; // Step 1 done
                                 if (address == 0x51) return 0; // Step 2 not done
                                 return 0;
                             });

            var reader = new GameReader(_mockMemoryReader.Object);
            var result = reader.ReadQuestSteps(quest);

            Assert.Equal(2, result.Count);
            Assert.Equal(255, result[1]);
            Assert.Equal(0, result[2]);
            Assert.True(quest.Requisites[0].IsDone);
        }
    }
}
