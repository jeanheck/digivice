namespace Tests.Memory.Readers;

using Xunit;
using Moq;
using Backend.Memory.Readers;
using Backend.Memory.Addresses;

public class ImportantItemsReaderTests
{
    [Fact]
    public void Read_ShouldMapImportantItemsResourceCorrectly()
    {
        var addresses = new ImportantItemsAddresses
        {
            TreeBoots = 0x00048DB4,
            FishingPole = 0x00048DB5,
            AsukaTrophy = 0x00048DC2,
            SunTrophy = 0x00048DC4
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DB4, 1)).Returns([(byte)0x01]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DB5, 1)).Returns([(byte)0x00]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DC2, 1)).Returns([(byte)0x02]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DC4, 1)).Returns([(byte)0x01]);

        var reader = new ImportantItemsReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.NotNull(result);
        Assert.Equal((byte)0x01, result.TreeBoots);
        Assert.Equal((byte)0x00, result.FishingPole);
        Assert.Equal((byte)0x02, result.AsukaTrophy);
        Assert.Equal((byte)0x01, result.SunTrophy);
    }
}
