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
            mockReader.Setup(r => r.ReadBytes(0x00048D88, 10))
                      .Returns([0x21, 0x12, 0x20, 0x21, 0x12, 0x00]);

            // Bits: 1234
            mockReader.Setup(r => r.ReadInt32(0x00048DA0))
                      .Returns(1234);

            // Mocking Slots: Empty (0xFF) to avoid IndexOutOfRangeException when GetPlayer calls GetParty
            mockReader.Setup(r => r.ReadBytes(0x00048DA4, 4)).Returns([0xFF]);
            mockReader.Setup(r => r.ReadBytes(0x00048DA8, 4)).Returns([0xFF]);
            mockReader.Setup(r => r.ReadBytes(0x00048DAC, 4)).Returns([0xFF]);

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
            mockReader.Setup(r => r.ReadBytes(0x00048DA4, 4)).Returns([0x00, 0x00, 0x00, 0x00]);
            // Mocking Slot 2: Renamon (ID 6)
            mockReader.Setup(r => r.ReadBytes(0x00048DA8, 4)).Returns([0x06, 0x00, 0x00, 0x00]);
            // Mocking Slot 3: Empty (0xFF)
            mockReader.Setup(r => r.ReadBytes(0x00048DAC, 4)).Returns([0xFF, 0x00, 0x00, 0x00]);

            // Kotemon Stats (Base 0x0004949C)
            int kotemonBase = 0x0004949C;
            mockReader.Setup(r => r.ReadInt16(kotemonBase + 0x1C)).Returns(15); // Level
            mockReader.Setup(r => r.ReadInt16(kotemonBase + 0x28)).Returns(100); // Attack
            mockReader.Setup(r => r.ReadInt16(kotemonBase + 0x34)).Returns(50);  // FireResist

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
            mockReader.Setup(r => r.ReadBytes(0x00048DA4, 4)).Returns([0x99, 0x00, 0x00, 0x00]);
            // Mocking Slots 2 & 3: Empty
            mockReader.Setup(r => r.ReadBytes(0x00048DA8, 4)).Returns([0xFF, 0x00, 0x00, 0x00]);
            mockReader.Setup(r => r.ReadBytes(0x00048DAC, 4)).Returns([0xFF, 0x00, 0x00, 0x00]);

            var service = new GameStateService(mockReader.Object);
            var party = service.GetParty();

            Assert.Empty(party.Digimons);
        }
    }
}
