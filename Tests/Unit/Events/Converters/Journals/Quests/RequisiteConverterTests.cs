namespace Tests.Events.Converters.Journals.Quests;

using Backend.Domain.Models.Journals.Quests;
using Backend.Events.Converters.Journals.Quests;

public class RequisiteConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllRequisiteFields()
    {
        var dto = RequisiteConverter.ToDTO(new Requisite { Id = "ReqA", Value = 8 });

        Assert.Equal("ReqA", dto.Id);
        Assert.Equal((byte)8, dto.Value.Value);
    }
}
