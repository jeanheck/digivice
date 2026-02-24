using Backend.Constants;
using Backend.Interfaces;
using Backend.Services;
using Moq;

namespace Tests.Backend.Services
{
    public class GameStateServiceTests
    {
        [Fact]
        public void GetState_ShouldReturnDecodedPlayerNameAndBits()
        {
            var mockReader = new Mock<IMemoryReaderService>();

            // "TESTE"
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.Name, PlayerAddresses.NameBufferSize))
                      .Returns([0x21, 0x12, 0x20, 0x21, 0x12, 0x00]);

            // Bits: 1234
            mockReader.Setup(r => r.ReadInt32(PlayerAddresses.Bits))
                      .Returns(1234);

            // Mocking Slots: Empty
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot1, PlayerAddresses.PartySlotStride)).Returns([DigimonAddresses.EmptySlotId]);
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot2, PlayerAddresses.PartySlotStride)).Returns([DigimonAddresses.EmptySlotId]);
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot3, PlayerAddresses.PartySlotStride)).Returns([DigimonAddresses.EmptySlotId]);

            var service = new GameStateService(mockReader.Object);
            var state = service.GetState();

            Assert.NotNull(state.Player);
            Assert.Equal("TESTE", state.Player.Name);
            Assert.Equal(1234, state.Player.Bits);
            Assert.NotNull(state.Party);
            Assert.All(state.Party.Slots, slot => Assert.Null(slot));
        }

        [Fact]
        public void GetState_ShouldReturnPartyWithCorrectSlotsAndNestedStats()
        {
            var mockReader = new Mock<IMemoryReaderService>();

            // Mock necessary player data so state creation doesn't crash on name decode
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.Name, PlayerAddresses.NameBufferSize)).Returns([0x00]);

            // Mocking Slot 1: Kotemon (ID 0)
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot1, PlayerAddresses.PartySlotStride)).Returns([(byte)DigimonIds.Kotemon, 0x00, 0x00, 0x00]);
            // Mocking Slot 2: Renamon (ID 6)
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot2, PlayerAddresses.PartySlotStride)).Returns([(byte)DigimonIds.Renamon, 0x00, 0x00, 0x00]);
            // Mocking Slot 3: Empty
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot3, PlayerAddresses.PartySlotStride)).Returns([DigimonAddresses.EmptySlotId, 0x00, 0x00, 0x00]);

            // Kotemon Stats
            int kotemonBase = DigimonAddresses.Digimons[(byte)DigimonIds.Kotemon].Address;
            mockReader.Setup(r => r.ReadInt16(kotemonBase + DigimonAddresses.BasicInfo.Level)).Returns(15);
            mockReader.Setup(r => r.ReadInt16(kotemonBase + DigimonAddresses.Attributes.Strength)).Returns(100);
            mockReader.Setup(r => r.ReadInt16(kotemonBase + DigimonAddresses.Resistances.Fire)).Returns(50);
            mockReader.Setup(r => r.ReadInt16(kotemonBase + DigimonAddresses.Equipments.Head)).Returns(215);

            var service = new GameStateService(mockReader.Object);
            var state = service.GetState();

            Assert.NotNull(state.Party);
            var activeSlots = state.Party.Slots.Where(d => d != null).ToList();
            Assert.Equal(2, activeSlots.Count);

            // Kotemon Assertions
            var kotemon = state.Party.Slots[0];
            Assert.NotNull(kotemon);
            Assert.Equal("Kotemon", kotemon.BasicInfo.Name);
            Assert.Equal(1, kotemon.SlotIndex);
            Assert.Equal(15, kotemon.BasicInfo.Level);
            Assert.Equal(100, kotemon.Attributes.Strength);
            Assert.Equal(50, kotemon.Resistances.Fire);
            Assert.Equal(215, kotemon.Equipments.Head);

            // Renamon Assertions
            var renamon = state.Party.Slots[1];
            Assert.NotNull(renamon);
            Assert.Equal("Renamon", renamon.BasicInfo.Name);
            Assert.Equal(2, renamon.SlotIndex);

            // Empty Slot (Index 2)
            Assert.Null(state.Party.Slots[2]);
        }

        [Fact]
        public void GetState_ShouldSkipUnknownIDsGracefullyForPartySlots()
        {
            var mockReader = new Mock<IMemoryReaderService>();

            // Mock necessary player data
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.Name, PlayerAddresses.NameBufferSize)).Returns([0x00]);

            // Mocking Slot 1: Unknown ID (0x99)
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot1, PlayerAddresses.PartySlotStride)).Returns([0x99, 0x00, 0x00, 0x00]);
            // Mocking Slots 2 & 3: Empty
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot2, PlayerAddresses.PartySlotStride)).Returns([DigimonAddresses.EmptySlotId, 0x00, 0x00, 0x00]);
            mockReader.Setup(r => r.ReadBytes(PlayerAddresses.PartySlot3, PlayerAddresses.PartySlotStride)).Returns([DigimonAddresses.EmptySlotId, 0x00, 0x00, 0x00]);

            var service = new GameStateService(mockReader.Object);
            var state = service.GetState();

            Assert.NotNull(state.Party);
            Assert.All(state.Party.Slots, slot => Assert.Null(slot));
        }
    }
}
