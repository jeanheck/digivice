namespace Tests.Events.Converters.Journals;

using Backend.Domain.Models.Journals;
using Backend.Domain.Models.Journals.Quests;
using Backend.Events.Converters.Journals;

public class QuestConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapQuestStepsAndRequisites()
    {
        var quest = new Quest
        {
            Id = "QuestA",
            Requisites = [new Requisite { Id = "ReqA", Value = 1 }],
            Steps =
            [
                new Step
                {
                    Number = 2,
                    Value = 3,
                    Requisites = [new Requisite { Id = "ReqB", Value = 4 }]
                }
            ]
        };

        var dto = QuestConverter.ToDTO(quest);

        Assert.Equal("QuestA", dto.Id);
        var requisite = Assert.Single(dto.Requisites.Value!);
        Assert.Equal("ReqA", requisite.Id);
        Assert.Equal((byte)1, requisite.Value.Value);
        var step = Assert.Single(dto.Steps.Value!);
        Assert.Equal(2, step.Number);
        Assert.Equal((byte)3, step.Value.Value);
        var nestedRequisite = Assert.Single(step.Requisites.Value!);
        Assert.Equal("ReqB", nestedRequisite.Id);
        Assert.Equal((byte)4, nestedRequisite.Value.Value);
    }
}
