using Backend.Constants;
using Backend.Interfaces;
using Backend.Services;
using Moq;

namespace Tests.Backend.Services
{
    public class GameStateServiceTests
    {
        [Fact]
        public void GetPlayer_ShouldReturnDecodedNameAndBits()
        {
            var mockReader = new Mock<IMemoryReader>();

            // "TESTE"
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.Name, PlayerAddresses.NameBufferSize))
                      .Returns([0x21, 0x12, 0x20, 0x21, 0x12, 0x00]);

            // Bits: 1234
            mockReader.Setup(r => r.ReadInt32(PlayerAddresses.Bits))
                      .Returns(1234);

            // Mocking Slots: Empty  to avoid IndexOutOfRangeException when GetPlayer calls GetParty
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot1, PlayerAddresses.PartySlotStride)).Returns([DigimonAddresses.EmptySlotId]);
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot2, PlayerAddresses.PartySlotStride)).Returns([DigimonAddresses.EmptySlotId]);
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot3, PlayerAddresses.PartySlotStride)).Returns([DigimonAddresses.EmptySlotId]);

            var service = new GameStateService(mockReader.Object);
            var player = service.GetPlayer();

            Assert.NotNull(player);
            Assert.Equal("TESTE", player.Name);
            Assert.Equal(1234, player.Bits);
            Assert.Empty(player.Party.Digimons);
        }

        [Fact]
        public void GetParty_ShouldHandleCorrectSlotsAndNestedStats()
        {
            var mockReader = new Mock<IMemoryReader>();

            // Mocking Slot 1: Kotemon (ID 0)
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot1, PlayerAddresses.PartySlotStride)).Returns([(byte)DigimonIds.Kotemon, 0x00, 0x00, 0x00]);
            // Mocking Slot 2: Renamon (ID 6)
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot2, PlayerAddresses.PartySlotStride)).Returns([(byte)DigimonIds.Renamon, 0x00, 0x00, 0x00]);
            // Mocking Slot 3: Empty
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot3, PlayerAddresses.PartySlotStride)).Returns([DigimonAddresses.EmptySlotId, 0x00, 0x00, 0x00]);

            // Kotemon Stats
            int kotemonBase = DigimonAddresses.Digimons[(byte)DigimonIds.Kotemon].Address;
            mockReader.Setup(r => r.ReadInt16(kotemonBase + DigimonAddresses.BasicInfo.Level)).Returns(15);
            mockReader.Setup(r => r.ReadInt16(kotemonBase + DigimonAddresses.Attributes.Attack)).Returns(100);
            mockReader.Setup(r => r.ReadInt16(kotemonBase + DigimonAddresses.Resistances.Fire)).Returns(50);

            var service = new GameStateService(mockReader.Object);
            var party = service.GetParty();

            Assert.Equal(2, party.Digimons.Count);

            // Kotemon Assertions
            var kotemon = party.Digimons[0];
            Assert.Equal("Kotemon", kotemon.BasicInfo.Name);
            Assert.Equal(1, kotemon.SlotIndex);
            Assert.Equal(15, kotemon.BasicInfo.Level);
            Assert.Equal(100, kotemon.Attributes.Attack);
            Assert.Equal(50, kotemon.Resistances.Fire);

            // Renamon Assertions
            var renamon = party.Digimons[1];
            Assert.Equal("Renamon", renamon.BasicInfo.Name);
            Assert.Equal(2, renamon.SlotIndex);
        }

        [Fact]
        public void GetParty_ShouldSkipUnknownIDsGracefully()
        {
            var mockReader = new Mock<IMemoryReader>();

            // Mocking Slot 1: Unknown ID (0x99)
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot1, PlayerAddresses.PartySlotStride)).Returns([0x99, 0x00, 0x00, 0x00]);
            // Mocking Slots 2 & 3: Empty
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot2, PlayerAddresses.PartySlotStride)).Returns([DigimonAddresses.EmptySlotId, 0x00, 0x00, 0x00]);
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot3, PlayerAddresses.PartySlotStride)).Returns([DigimonAddresses.EmptySlotId, 0x00, 0x00, 0x00]);

            var service = new GameStateService(mockReader.Object);
            var party = service.GetParty();

            Assert.Empty(party.Digimons);
        }
    }
}
