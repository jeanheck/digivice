namespace Tests.Memory.Readers.Journals.Quests;

using Xunit;
using Moq;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Journals.Quests;
using Backend.Memory.Addresses.Journals.Quests;

public class RequisiteReaderTests
{
    [Fact]
    public void Read_ShouldMapRequisiteResourceCorrectly()
    {
        var addresses = new RequisiteAddresses
        {
            Id = "ReqX",
            Address = 0x7500
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x7500, 1)).Returns([(byte)7]);

        var reader = new RequisiteReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.NotNull(result);
        Assert.Equal("ReqX", result.Id);
        Assert.Equal(7, result.Value);
    }

    [Fact]
    public void Read_ShouldReturnRawByte_WhenBitMasksIsEmpty()
    {
        var addresses = new RequisiteAddresses
        {
            Id = "FolderBag",
            Address = 0x48F42,
            BitMasks = []
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x48F42, 1)).Returns([(byte)1]);

        var reader = new RequisiteReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.Equal(1, result.Value);
    }

    [Fact]
    public void Read_ShouldApplyBitMask_WhenSingleBitMaskIsProvided()
    {
        var addresses = new RequisiteAddresses
        {
            Id = "byakkoLeader",
            Address = 0x4B3E5,
            BitMasks = [0x40]
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x4B3E5, 1)).Returns([(byte)0x51]);

        var reader = new RequisiteReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.Equal(0x40, result.Value);
    }

    [Fact]
    public void Read_ShouldReturnZero_WhenNotAllBitMasksAreSet()
    {
        var addresses = new RequisiteAddresses
        {
            Id = "ReqMulti",
            Address = 0x4B370,
            BitMasks = [0x01, 0x08]
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x4B370, 1)).Returns([(byte)0x01]);

        var reader = new RequisiteReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Read_ShouldReturnFirstBitMaskValue_WhenAllBitMasksAreSet()
    {
        var addresses = new RequisiteAddresses
        {
            Id = "ReqMulti",
            Address = 0x4B370,
            BitMasks = [0x01, 0x08]
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x4B370, 1)).Returns([(byte)0x09]);

        var reader = new RequisiteReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.Equal(0x01, result.Value);
    }
}
