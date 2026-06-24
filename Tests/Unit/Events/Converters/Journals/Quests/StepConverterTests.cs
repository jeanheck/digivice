namespace Tests.Events.Converters.Journals.Quests;

using Backend.Domain.Models.Journals.Quests;
using Backend.Events.Converters.Journals.Quests;

public class StepConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapStepAndNestedRequisites()
    {
        var step = new Step
        {
            Number = 5,
            Value = 6,
            Requisites = [new Requisite { Id = "ReqA", Value = 7 }]
        };

        var dto = StepConverter.ToDTO(step);

        Assert.Equal(5, dto.Number);
        Assert.Equal((byte)6, dto.Value.Value);
        var requisite = Assert.Single(dto.Requisites.Value!);
        Assert.Equal("ReqA", requisite.Id);
        Assert.Equal((byte)7, requisite.Value.Value);
    }
}
