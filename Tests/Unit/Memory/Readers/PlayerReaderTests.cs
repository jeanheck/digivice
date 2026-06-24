namespace Tests.Memory.Readers;

using Xunit;
using Moq;
using Backend.Memory.Readers;
using Backend.Memory.Addresses;

public class PlayerReaderTests
{
    [Fact]
    public void Read_ShouldMapPlayerResourceCorrectly()
    {
        // Arrange
        var addresses = new PlayerAddresses
        {
            Bits = 0x1000,
            Name = 0x2000,
            NameBufferSize = 10,
            MapId = 0x3000
        };

        var nameBytes = new byte[] { 65, 103, 117, 109, 111, 110, 0, 0, 0, 0 }; // "Agumon"

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadInt32(0x1000)).Returns(15000);
        memoryReaderMock.Setup(m => m.ReadBytes(0x2000, 10)).Returns(nameBytes);
        memoryReaderMock.Setup(m => m.ReadInt16(0x3000)).Returns((short)4);

        var reader = new PlayerReader(memoryReaderMock.Object);

        // Act
        var result = reader.Read(addresses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(15000, result.Bits);
        Assert.Equal(nameBytes, result.NameInBytes);
        Assert.Equal((short)4, result.MapId);
    }
}
