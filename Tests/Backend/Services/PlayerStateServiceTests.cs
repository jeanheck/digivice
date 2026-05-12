using Backend.Interfaces;
using Backend.Models.Addresses;
using Backend.Models.Resources;
using Backend.Services;
using Moq;

namespace Tests.Backend.Services
{
    public class PlayerStateServiceTests
    {
        private readonly Mock<IGameDatabase> _mockDatabase;
        private readonly Mock<IGameReader> _mockReader;
        private readonly PlayerStateService _playerService;

        public PlayerStateServiceTests()
        {
            _mockDatabase = new Mock<IGameDatabase>();
            _mockReader = new Mock<IGameReader>();
            _playerService = new PlayerStateService(_mockDatabase.Object, _mockReader.Object);
        }

        [Fact]
        public void GetPlayer_ShouldReturnNull_WhenNameBytesIsNull()
        {
            _mockDatabase.Setup(db => db.GetPlayerAddresses()).Returns(new PlayerAddresses());
            _mockReader.Setup(r => r.ReadPlayer(It.IsAny<PlayerAddresses>()))
                       .Returns(new PlayerResource { NameInBytes = null });

            var result = _playerService.GetPlayer();

            Assert.Null(result);
        }

        [Fact]
        public void GetPlayer_ShouldMapAndFormatHex_ValuesCorrectly()
        {
            byte[] encodedName = [0x1A, 0x2C, 0x01, 0x00]; // "Me " -> TextDecoder handles it

            _mockDatabase.Setup(db => db.GetPlayerAddresses()).Returns(new PlayerAddresses());
            _mockReader.Setup(r => r.ReadPlayer(It.IsAny<PlayerAddresses>()))
                       .Returns(new PlayerResource
                       {
                           NameInBytes = encodedName,
                           Bits = 15500,
                           MapId = 255 // Hex string should be "00FF"
                       });

            var result = _playerService.GetPlayer();

            Assert.NotNull(result);
            Assert.Equal("Me", result.Name);
            Assert.Equal(15500, result.Bits);
            Assert.Equal("00FF", result.MapId);
        }
    }
}
