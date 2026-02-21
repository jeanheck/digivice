using Backend.Interfaces;
using Backend.Services;
using Moq;

namespace Tests.Backend.Services
{
    public class GameStateServiceTests
    {
        [Fact]
        public void GetPlayer_ShouldReturnDecodedName()
        {
            var mockReader = new Mock<IMemoryReader>();
            // "TESTE"
            mockReader.Setup(r => r.ReadBytes(0x00048D88, 10))
                      .Returns(new byte[] { 0x21, 0x12, 0x20, 0x21, 0x12, 0x00 });

            var service = new GameStateService(mockReader.Object);
            var player = service.GetPlayer();

            Assert.NotNull(player);
            Assert.Equal("TESTE", player.Name);
        }

        [Fact]
        public void GetParty_ShouldReturnCorrectDigimons()
        {
            var mockReader = new Mock<IMemoryReader>();

            // IDs: 0 (Kotemon), 6 (Renamon), 7 (Patamon)
            // Stored as Int32 (4-byte alignment): 00 00 00 00 | 06 00 00 00 | 07 00 00 00
            byte[] mockMemory = new byte[]
            {
                0x00, 0x00, 0x00, 0x00,
                0x06, 0x00, 0x00, 0x00,
                0x07, 0x00, 0x00, 0x00
            };

            mockReader.Setup(r => r.ReadBytes(0x00048DA4, 12))
                      .Returns(mockMemory);

            var service = new GameStateService(mockReader.Object);
            var party = service.GetParty();

            Assert.Equal(3, party.Digimons.Count);
            Assert.Equal("Kotemon", party.Digimons[0].Name);
            Assert.Equal("Renamon", party.Digimons[1].Name);
            Assert.Equal("Patamon", party.Digimons[2].Name);
        }
    }
}
