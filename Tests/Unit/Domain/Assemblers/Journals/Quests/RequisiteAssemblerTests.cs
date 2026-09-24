namespace Tests.Domain.Assemblers.Journals.Quests;

using Backend.Domain.Assemblers.Journals.Quests;
using Backend.Memory.Resources.Journals.Quests;

public class RequisiteAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAllFieldsCorrectly()
    {
        var resource = new RequisiteResource
        {
            Id = "5",
            Value = 1
        };

        var result = RequisiteAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal("5", result.Id);
        Assert.True(result.IsDone);
    }

    [Theory]
    [InlineData(0x00, false)]
    [InlineData(0x01, true)]
    [InlineData(0x80, true)]
    public void Assemble_ShouldMapIsDoneFromNonZeroValue(byte value, bool expectedIsDone)
    {
        var result = RequisiteAssembler.Assemble(new RequisiteResource { Id = "5", Value = value });

        Assert.Equal(expectedIsDone, result.IsDone);
    }
}
