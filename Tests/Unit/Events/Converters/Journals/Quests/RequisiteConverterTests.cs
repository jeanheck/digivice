namespace Tests.Events.Converters.Journals.Quests;

using Backend.Domain.Models.Journals.Quests;
using Backend.Events.Converters.Journals.Quests;

public class RequisiteConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllRequisiteFields()
    {
        var dto = RequisiteConverter.ToDTO(new Requisite { Id = "ReqA", IsDone = true });

        Assert.Equal("ReqA", dto.Id);
        Assert.True(dto.IsDone.Value);
    }
}
