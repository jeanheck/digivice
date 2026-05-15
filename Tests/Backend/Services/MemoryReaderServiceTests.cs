using Backend.Services;
using Backend.Interfaces;
using Moq;
using Microsoft.Extensions.Configuration;

namespace Tests.Backend.Services
{
    public class MemoryReaderServiceTests
    {
        private readonly Mock<IProcessService> _mockProcessService;
        private readonly Mock<IMemoryProvider> _mockMemoryProvider;
        private readonly Mock<IMemoryAccessor> _mockMemoryAccessor;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public MemoryReaderServiceTests()
        {
            _mockProcessService = new Mock<IProcessService>();
            _mockMemoryProvider = new Mock<IMemoryProvider>();
            _mockMemoryAccessor = new Mock<IMemoryAccessor>();
            _mockConfiguration = new Mock<IConfiguration>();

            var mockConfigSection = new Mock<IConfigurationSection>();
            mockConfigSection.Setup(s => s.Value).Returns("duckstation-qt");
            _mockConfiguration.Setup(c => c.GetSection("EmulatorProcessName")).Returns(mockConfigSection.Object);
        }

        [Fact]
        public void TryConnect_ShouldReturnTrue_WhenProcessExists()
        {
            _mockProcessService.Setup(p => p.GetProcessIdByName(It.IsAny<string>())).Returns(1234);
            _mockMemoryProvider.Setup(p => p.OpenExisting(It.IsAny<string>())).Returns(_mockMemoryAccessor.Object);

            var reader = new MemoryReaderService(_mockProcessService.Object, _mockMemoryProvider.Object, _mockConfiguration.Object);
            Assert.True(reader.TryConnect());
            Assert.True(reader.IsConnected);
        }

        [Fact]
        public void ReadBytes_ShouldReturnArray_WhenConnectedAndMapped()
        {
            byte[] expectedData = [0xAA, 0xBB, 0xCC];
            _mockProcessService.Setup(p => p.GetProcessIdByName(It.IsAny<string>())).Returns(1234);
            _mockMemoryProvider.Setup(p => p.OpenExisting(It.IsAny<string>())).Returns(_mockMemoryAccessor.Object);

            _mockMemoryAccessor.Setup(a => a.ReadArray(0x1000, It.IsAny<byte[]>(), 0, 3))
                               .Callback<long, byte[], int, int>((addr, buf, idx, count) =>
                               {
                                   expectedData.CopyTo(buf, idx);
                               });

            var reader = new MemoryReaderService(_mockProcessService.Object, _mockMemoryProvider.Object, _mockConfiguration.Object);
            reader.TryConnect();

            var result = reader.ReadBytes(0x1000, 3);
            Assert.Equal(expectedData, result);
        }

        [Fact]
        public void ReadInt32_ShouldReturnParsedValue_WhenConnected()
        {
            _mockProcessService.Setup(p => p.GetProcessIdByName(It.IsAny<string>())).Returns(1234);
            _mockMemoryProvider.Setup(p => p.OpenExisting(It.IsAny<string>())).Returns(_mockMemoryAccessor.Object);
            _mockMemoryAccessor.Setup(a => a.ReadInt32(0x3000)).Returns(0x11223344);

            var reader = new MemoryReaderService(_mockProcessService.Object, _mockMemoryProvider.Object, _mockConfiguration.Object);
            reader.TryConnect();

            Assert.Equal(0x11223344, reader.ReadInt32(0x3000));
        }

        [Fact]
        public void ReadInt16_ShouldReturnParsedValue_WhenConnected()
        {
            _mockProcessService.Setup(p => p.GetProcessIdByName(It.IsAny<string>())).Returns(1234);
            _mockMemoryProvider.Setup(p => p.OpenExisting(It.IsAny<string>())).Returns(_mockMemoryAccessor.Object);
            _mockMemoryAccessor.Setup(a => a.ReadInt16(0x4000)).Returns((short)0x1234);

            var reader = new MemoryReaderService(_mockProcessService.Object, _mockMemoryProvider.Object, _mockConfiguration.Object);
            reader.TryConnect();

            Assert.Equal((short)0x1234, reader.ReadInt16(0x4000));
        }

        [Fact]
        public void ReadByteSafe_ShouldReturnByte_OrZeroIfAddressInvalid()
        {
            byte[] expectedData = { 0xAA };
            _mockProcessService.Setup(p => p.GetProcessIdByName(It.IsAny<string>())).Returns(1234);
            _mockMemoryProvider.Setup(p => p.OpenExisting(It.IsAny<string>())).Returns(_mockMemoryAccessor.Object);
            _mockMemoryAccessor.Setup(a => a.ReadArray(0x50, It.IsAny<byte[]>(), 0, 1))
                               .Callback<long, byte[], int, int>((addr, buf, idx, count) => expectedData.CopyTo(buf, idx));

            var reader = new MemoryReaderService(_mockProcessService.Object, _mockMemoryProvider.Object, _mockConfiguration.Object);
            reader.TryConnect();

            Assert.Equal(0xAA, reader.ReadByteSafe(0x50));
            Assert.Equal(0, reader.ReadByteSafe(0));
        }
    }
}
