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
            IsDone = true,
            Requisites = [new Requisite { Id = "ReqA", IsDone = true }]
        };

        var dto = StepConverter.ToDTO(step);

        Assert.Equal(5, dto.Number);
        Assert.True(dto.IsDone.Value);
        var requisite = Assert.Single(dto.Requisites.Value!);
        Assert.Equal("ReqA", requisite.Id);
        Assert.True(requisite.IsDone.Value);
    }
}
