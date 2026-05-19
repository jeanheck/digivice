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
                Steps = [new Step { Number = 1, Value = 1 }],
                Requisites = []
            },
            SideQuests =
            [
                new Quest
                {
                    Id = "FolderBag",
                    Steps = [new Step { Number = 1, Value = 0 }],
                    Requisites = []
                }
            ]
        };

        var dto = JournalConverter.ToDTO(journal);

        Assert.True(dto.MainQuest.HasValue);
        Assert.Equal("MainQuest", dto.MainQuest.Value!.Id);
        Assert.True(dto.MainQuest.Value.Steps.HasValue);
        Assert.Equal(1, dto.MainQuest.Value.Steps.Value![0].Number);
        Assert.Equal((byte)1, dto.MainQuest.Value.Steps.Value[0].Value.Value);

        Assert.True(dto.SideQuests.HasValue);
        var sideQuest = Assert.Single(dto.SideQuests.Value!);
        Assert.Equal("FolderBag", sideQuest.Id);
        Assert.True(sideQuest.Steps.HasValue);
        Assert.Equal((byte)0, sideQuest.Steps.Value![0].Value.Value);
    }
}
