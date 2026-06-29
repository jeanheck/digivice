namespace Tests.Memory.Readers.Journals;

using Xunit;
using Moq;
using Backend.Memory.Readers.Journals;
using Backend.Memory.Readers.Journals.Quests;
using Backend.Memory.Addresses.Journals;
using Backend.Memory.Addresses.Journals.Quests;
using Backend.Memory.Resources.Journals.Quests;

public class QuestReaderTests
{
    [Fact]
    public void Read_ShouldMapQuestResourceCorrectly()
    {
        // Arrange
        var reqAddresses = new RequisiteAddresses { Id = "Req1", Address = 0x4000 };
        var stepAddresses = new StepAddresses { Number = 1, Address = 0x5000 };

        var addresses = new QuestAddresses
        {
            Id = "QuestA",
            Requisites = [reqAddresses],
            Steps = [stepAddresses]
        };

        var expectedRequisiteResource = new RequisiteResource { Id = "Req1", Value = 1 };
        var expectedStepResource = new StepResource { Number = 1, Value = 3 };

        var requisiteReaderMock = new Mock<IRequisiteReader>();
        requisiteReaderMock.Setup(r => r.Read(reqAddresses)).Returns(expectedRequisiteResource);

        var stepReaderMock = new Mock<IStepReader>();
        stepReaderMock.Setup(s => s.Read(stepAddresses)).Returns(expectedStepResource);

        var reader = new QuestReader(requisiteReaderMock.Object, stepReaderMock.Object);

        // Act
        var result = reader.Read(addresses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("QuestA", result.Id);
        Assert.Single(result.Requisites);
        Assert.Equal(expectedRequisiteResource, result.Requisites[0]);
        Assert.Single(result.Steps);
        Assert.Equal(expectedStepResource, result.Steps[0]);
    }
}
