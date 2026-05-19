namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Domain.Models.Journals.Quests;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;

public class JournalEventFactoryTests
{
    [Fact]
    public void Create_ShouldReturnNoEvents_WhenJournalHasNoChanges()
    {
        var previousState = CreateState(CreateJournal(0));
        var newState = CreateState(CreateJournal(0));

        var result = JournalEventFactory.Create(previousState, newState);

        Assert.Empty(result);
    }

    [Fact]
    public void Create_ShouldReturnJournalChangedEvent_WhenJournalChanges()
    {
        var previousState = CreateState(CreateJournal(0));
        var newState = CreateState(CreateJournal(1));

        var result = JournalEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.JournalChanged, ev.Type);

        var dto = Assert.IsType<JournalDTO>(ev.Data);
        Assert.True(dto.MainQuest.HasValue);
        Assert.Equal("MainQuest", dto.MainQuest.Value!.Id);
    }

    private static State CreateState(Journal journal)
    {
        return new State { Journal = journal };
    }

    private static Journal CreateJournal(byte stepValue)
    {
        return new Journal
        {
            MainQuest = new Quest
            {
                Id = "MainQuest",
                Steps = [new Step { Number = 1, Value = stepValue }],
                Requisites = []
            },
            SideQuests = []
        };
    }
}
