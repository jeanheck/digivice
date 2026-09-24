namespace Tests.Domain.Assemblers.Journals.Quests;

using Backend.Domain.Assemblers.Journals.Quests;
using Backend.Memory.Resources.Journals.Quests;

public class StepAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAllFieldsCorrectly()
    {
        var resource = new StepResource
        {
            Number = 3,
            Value = 1,
            Requisites = [
                new RequisiteResource { Id = "1", Value = 1 }
            ]
        };

        var result = StepAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(3, result.Number);
        Assert.True(result.IsDone);
        Assert.Single(result.Requisites);
        Assert.Equal("1", result.Requisites[0].Id);
    }

    [Theory]
    [InlineData(0x00, false)]
    [InlineData(0x01, true)]
    [InlineData(0x80, true)]
    public void Assemble_ShouldMapIsDoneFromNonZeroValue(byte value, bool expectedIsDone)
    {
        var result = StepAssembler.Assemble(new StepResource { Number = 1, Value = value, Requisites = [] });

        Assert.Equal(expectedIsDone, result.IsDone);
    }
}
