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
            BitMask = 0x0F,
            Requisites = [reqAddresses]
        };

        var expectedRequisiteResource = new RequisiteResource { Id = "Req2", Value = 5 };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadByteSafe(0x5500, 0x0F)).Returns((byte)2);

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
}
