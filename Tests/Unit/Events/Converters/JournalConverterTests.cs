namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Domain.Models.Journals.Quests;
using Backend.Events.Converters;

public class JournalConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapMainQuestAndSideQuests()
    {
        var journal = new Journal
        {
            MainQuest = new Quest
            {
                Id = "MainQuest",
                Steps = [new Step { Number = 1, IsDone = true }],
                Requisites = []
            },
            SideQuests =
            [
                new Quest
                {
                    Id = "folderBag",
                    Steps = [new Step { Number = 1, IsDone = false }],
                    Requisites = []
                }
            ]
        };

        var dto = JournalConverter.ToDTO(journal);

        Assert.True(dto.MainQuest.HasValue);
        Assert.Equal("MainQuest", dto.MainQuest.Value!.Id);
        Assert.True(dto.MainQuest.Value.Steps.HasValue);
        Assert.Equal(1, dto.MainQuest.Value.Steps.Value![0].Number);
        Assert.True(dto.MainQuest.Value.Steps.Value[0].IsDone.Value);

        Assert.True(dto.SideQuests.HasValue);
        var sideQuest = Assert.Single(dto.SideQuests.Value!);
        Assert.Equal("folderBag", sideQuest.Id);
        Assert.True(sideQuest.Steps.HasValue);
        Assert.False(sideQuest.Steps.Value![0].IsDone.Value);
    }
}
