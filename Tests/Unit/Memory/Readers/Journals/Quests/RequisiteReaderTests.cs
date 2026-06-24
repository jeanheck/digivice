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
        // Arrange
        var addresses = new RequisiteAddresses
        {
            Id = "ReqX",
            Address = 0x7500
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x7500, 1)).Returns([(byte)7]);

        var reader = new RequisiteReader(memoryReaderMock.Object);

        // Act
        var result = reader.Read(addresses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ReqX", result.Id);
        Assert.Equal(7, result.Value);
    }
}
