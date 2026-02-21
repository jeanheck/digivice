using Backend;
using Backend.Interfaces;
using Moq;
using Xunit;

namespace Tests.Backend
{
    public class MemoryReaderTests
    {
        private readonly Mock<IProcessService> _mockProcessService;
        private readonly Mock<IMemoryProvider> _mockMemoryProvider;
        private readonly Mock<IMemoryAccessor> _mockMemoryAccessor;

        public MemoryReaderTests()
        {
            _mockProcessService = new Mock<IProcessService>();
            _mockMemoryProvider = new Mock<IMemoryProvider>();
            _mockMemoryAccessor = new Mock<IMemoryAccessor>();
        }

        [Fact]
        public void TryConnect_ShouldReturnTrue_WhenProcessAndMappingExist()
        {
            // Arrange
            _mockProcessService.Setup(p => p.GetProcessIdByName(It.IsAny<string>()))
                               .Returns(1234);
            _mockMemoryProvider.Setup(p => p.OpenExisting("duckstation_1234"))
                               .Returns(_mockMemoryAccessor.Object);

            var reader = new MemoryReader(_mockProcessService.Object, _mockMemoryProvider.Object);

            // Act
            bool result = reader.TryConnect();

            // Assert
            Assert.True(result);
            Assert.True(reader.IsConnected);
        }

        [Fact]
        public void TryConnect_ShouldReturnFalse_WhenProcessNotFound()
        {
            // Arrange
            _mockProcessService.Setup(p => p.GetProcessIdByName(It.IsAny<string>()))
                               .Returns((int?)null);

            var reader = new MemoryReader(_mockProcessService.Object, _mockMemoryProvider.Object);

            // Act
            bool result = reader.TryConnect();

            // Assert
            Assert.False(result);
            Assert.False(reader.IsConnected);
        }

        [Fact]
        public void ReadInt32_ShouldReturnCorrectValue_WhenConnected()
        {
            // Arrange
            _mockProcessService.Setup(p => p.GetProcessIdByName(It.IsAny<string>()))
                               .Returns(1234);
            _mockMemoryProvider.Setup(p => p.OpenExisting(It.IsAny<string>()))
                               .Returns(_mockMemoryAccessor.Object);
            _mockMemoryAccessor.Setup(a => a.ReadInt32(0x100))
                               .Returns(999);

            var reader = new MemoryReader(_mockProcessService.Object, _mockMemoryProvider.Object);
            reader.TryConnect();

            // Act
            int result = reader.ReadInt32(0x100);

            // Assert
            Assert.Equal(999, result);
        }

        [Fact]
        public void ReadBytes_ShouldReturnBuffer_WhenConnected()
        {
            // Arrange
            byte[] expectedData = new byte[] { 0x01, 0x02, 0x03 };
            _mockProcessService.Setup(p => p.GetProcessIdByName(It.IsAny<string>()))
                               .Returns(1234);
            _mockMemoryProvider.Setup(p => p.OpenExisting(It.IsAny<string>()))
                               .Returns(_mockMemoryAccessor.Object);

            _mockMemoryAccessor.Setup(a => a.ReadArray(0x200, It.IsAny<byte[]>(), 0, 3))
                               .Callback<int, byte[], int, int>((addr, buf, idx, count) =>
                               {
                                   expectedData.CopyTo(buf, idx);
                               });

            var reader = new MemoryReader(_mockProcessService.Object, _mockMemoryProvider.Object);
            reader.TryConnect();

            // Act
            var result = reader.ReadBytes(0x200, 3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedData, result);
        }

        [Fact]
        public void ReadInt16_ShouldReturnCorrectValue_WhenConnected()
        {
            // Arrange
            _mockProcessService.Setup(p => p.GetProcessIdByName(It.IsAny<string>()))
                               .Returns(1234);
            _mockMemoryProvider.Setup(p => p.OpenExisting(It.IsAny<string>()))
                               .Returns(_mockMemoryAccessor.Object);
            _mockMemoryAccessor.Setup(a => a.ReadInt16(0x150))
                               .Returns(42);

            var reader = new MemoryReader(_mockProcessService.Object, _mockMemoryProvider.Object);
            reader.TryConnect();

            // Act
            short result = reader.ReadInt16(0x150);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void ReadString_ShouldReturnCorrectValue_WhenConnected()
        {
            // Arrange
            string expected = "HELLO";
            byte[] data = System.Text.Encoding.ASCII.GetBytes(expected);

            _mockProcessService.Setup(p => p.GetProcessIdByName(It.IsAny<string>()))
                               .Returns(1234);
            _mockMemoryProvider.Setup(p => p.OpenExisting(It.IsAny<string>()))
                               .Returns(_mockMemoryAccessor.Object);

            _mockMemoryAccessor.Setup(a => a.ReadArray(0x300, It.IsAny<byte[]>(), 0, 5))
                               .Callback<int, byte[], int, int>((addr, buf, idx, count) =>
                               {
                                   data.CopyTo(buf, idx);
                               });

            var reader = new MemoryReader(_mockProcessService.Object, _mockMemoryProvider.Object);
            reader.TryConnect();

            // Act
            string result = reader.ReadString(0x300, 5);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReadString_ShouldHandleNullTerminator()
        {
            // Arrange
            string expected = "ABC";
            byte[] data = new byte[] { (byte)'A', (byte)'B', (byte)'C', 0x00, (byte)'X' };

            _mockProcessService.Setup(p => p.GetProcessIdByName(It.IsAny<string>()))
                               .Returns(1234);
            _mockMemoryProvider.Setup(p => p.OpenExisting(It.IsAny<string>()))
                               .Returns(_mockMemoryAccessor.Object);

            _mockMemoryAccessor.Setup(a => a.ReadArray(0x400, It.IsAny<byte[]>(), 0, 5))
                               .Callback<int, byte[], int, int>((addr, buf, idx, count) =>
                               {
                                   data.CopyTo(buf, idx);
                               });

            var reader = new MemoryReader(_mockProcessService.Object, _mockMemoryProvider.Object);
            reader.TryConnect();

            // Act
            string result = reader.ReadString(0x400, 5);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReadInt32_ShouldReturnMinusOne_WhenNotConnected()
        {
            // Arrange
            var reader = new MemoryReader(_mockProcessService.Object, _mockMemoryProvider.Object);

            // Act
            int result = reader.ReadInt32(0x100);

            // Assert
            Assert.Equal(-1, result);
        }
    }
}
