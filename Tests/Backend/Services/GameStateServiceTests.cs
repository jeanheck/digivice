using Backend.Interfaces;
using Backend.Services;
using Backend.Models.Addresses;
using Moq;

namespace Tests.Backend.Services
{
    public class GameReaderTests
    {
        [Fact]
        public void ReadPlayer_ShouldReturnDecodedPlayerNameAndBits()
        {
            var mockReader = new Mock<IMemoryReaderService>();

            // Setup a fake memory read for name: "TESTE" (Digimon encoding)
            mockReader.Setup(r => r.ReadBytes(0x10, 6))
                      .Returns([0x21, 0x12, 0x20, 0x21, 0x12, 0x00]);

            // Setup a fake memory read for Bits: 1234
            mockReader.Setup(r => r.ReadInt32(0x20))
                      .Returns(1234);

            var reader = new GameReader(mockReader.Object);

            // Faking the JSON Model output by instantiating directly in-memory
            var fakeAddresses = new PlayerAddresses
            {
                Name = "0x10",
                NameBufferSize = 6,
                Bits = "0x20",
                MapIdAddress = "0x30"
            };

            var player = reader.ReadPlayer(fakeAddresses);

            Assert.NotNull(player);
            Assert.NotNull(player.NameBytes);
            // Note: The assertion here is simplified. Actually PlayerResource holds NameBytes and Bits. 
            Assert.Equal(6, player.NameBytes.Length);
            Assert.Equal(0x21, player.NameBytes[0]);
            Assert.Equal(1234, player.Bits);
        }

        [Fact]
        public void ReadParty_ShouldReturnPartyWithCorrectSlots()
        {
            var mockReader = new Mock<IMemoryReaderService>();

            // Setup fake memory reads for slots
            // Kotemon (ID 0) in Slot 1
            mockReader.Setup(r => r.ReadBytes(0x100, 4)).Returns([0x00, 0x00, 0x00, 0x00]);
            // Renamon (ID 6) in Slot 2
            mockReader.Setup(r => r.ReadBytes(0x200, 4)).Returns([0x06, 0x00, 0x00, 0x00]);
            // Empty (ID 0xFF) in Slot 3
            mockReader.Setup(r => r.ReadBytes(0x300, 4)).Returns([0xFF, 0x00, 0x00, 0x00]);

            var reader = new GameReader(mockReader.Object);

            // Faking the JSON Model output
            var fakeAddresses = new PartyAddresses
            {
                PartySlot1 = "0x100",
                PartySlot2 = "0x200",
                PartySlot3 = "0x300",
                PartySlotStride = 4
            };

            var party = reader.ReadParty(fakeAddresses);

            Assert.NotNull(party);
            Assert.Equal(3, party.ActiveDigimonIds.Count);
            Assert.Equal(0, party.ActiveDigimonIds[0]);  // Kotemon
            Assert.Equal(6, party.ActiveDigimonIds[1]);  // Renamon
            Assert.Equal(255, party.ActiveDigimonIds[2]); // Empty
        }
    }
}
