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
            Requisites = [new Requisite { Id = "ReqA", IsDone = true }],
            Steps =
            [
                new Step
                {
                    Number = 2,
                    IsDone = true,
                    Requisites = [new Requisite { Id = "ReqB", IsDone = true }]
                }
            ]
        };

        var dto = QuestConverter.ToDTO(quest);

        Assert.Equal("QuestA", dto.Id);
        var requisite = Assert.Single(dto.Requisites.Value!);
        Assert.Equal("ReqA", requisite.Id);
        Assert.True(requisite.IsDone.Value);
        var step = Assert.Single(dto.Steps.Value!);
        Assert.Equal(2, step.Number);
        Assert.True(step.IsDone.Value);
        var nestedRequisite = Assert.Single(step.Requisites.Value!);
        Assert.Equal("ReqB", nestedRequisite.Id);
        Assert.True(nestedRequisite.IsDone.Value);
    }
}
