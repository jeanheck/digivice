namespace Tests.Memory.Readers.Journals.Quests;

using Xunit;
using Moq;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Journals.Quests;
using Backend.Memory.Addresses.Journals.Quests;
using Backend.Memory.Resources.Journals.Quests;

public class StepReaderTests
{
    [Fact]
    public void Read_ShouldMapStepResourceCorrectly()
    {
        // Arrange
        var reqAddresses = new RequisiteAddresses { Id = "Req2", Address = 0x6000 };
        var addresses = new StepAddresses
        {
            Number = 3,
            Address = 0x5500,
            BitMasks = [0x0F],
            Requisites = [reqAddresses]
        };

        var expectedRequisiteResource = new RequisiteResource { Id = "Req2", Value = 5 };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x5500, 1)).Returns([(byte)2]);

        var requisiteReaderMock = new Mock<IRequisiteReader>();
        requisiteReaderMock.Setup(r => r.Read(reqAddresses)).Returns(expectedRequisiteResource);

        var reader = new StepReader(memoryReaderMock.Object, requisiteReaderMock.Object);

        // Act
        var result = reader.Read(addresses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Number);
        Assert.Equal(2, result.Value);
        Assert.Single(result.Requisites);
        Assert.Equal(expectedRequisiteResource, result.Requisites[0]);
    }

    [Fact]
    public void Read_ShouldReturnZero_WhenNotAllBitMasksAreSet()
    {
        // Arrange
        var addresses = new StepAddresses
        {
            Number = 21,
            Address = 0x4B370,
            BitMasks = [0x01, 0x08]
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x4B370, 1)).Returns([(byte)0x01]);

        var requisiteReaderMock = new Mock<IRequisiteReader>();
        var reader = new StepReader(memoryReaderMock.Object, requisiteReaderMock.Object);

        // Act
        var result = reader.Read(addresses);

        // Assert
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Read_ShouldReturnFirstBitMaskValue_WhenAllBitMasksAreSet()
    {
        // Arrange
        var addresses = new StepAddresses
        {
            Number = 21,
            Address = 0x4B370,
            BitMasks = [0x01, 0x08]
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x4B370, 1)).Returns([(byte)0x09]);

        var requisiteReaderMock = new Mock<IRequisiteReader>();
        var reader = new StepReader(memoryReaderMock.Object, requisiteReaderMock.Object);

        // Act
        var result = reader.Read(addresses);

        // Assert
        Assert.Equal(0x01, result.Value);
    }

    [Fact]
    public void Read_ShouldReturnRawByte_WhenBitMasksIsEmpty()
    {
        // Arrange
        var addresses = new StepAddresses
        {
            Number = 1,
            Address = 0x48F42,
            BitMasks = []
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x48F42, 1)).Returns([(byte)1]);

        var requisiteReaderMock = new Mock<IRequisiteReader>();
        var reader = new StepReader(memoryReaderMock.Object, requisiteReaderMock.Object);

        // Act
        var result = reader.Read(addresses);

        // Assert
        Assert.Equal(1, result.Value);
    }
}
