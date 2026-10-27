namespace Tests.Memory.Readers;

using Backend.Memory.Addresses;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Interfaces;
using Moq;

public class CardInventoryReaderTests
{
    [Fact]
    public void Read_ShouldMapCardInventoryResourceCorrectly()
    {
        var addresses = new CardInventoryAddresses
        {
            Start = 0x000490D7,
            TotalCards = 314
        };

        byte[] fakeBytes = new byte[314];
        fakeBytes[9] = 1;

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x000490D7, 314)).Returns(fakeBytes);

        var reader = new CardInventoryReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.NotNull(result);
        Assert.Equal(fakeBytes, result.Quantities);
    }
}
