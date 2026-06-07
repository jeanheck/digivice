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

        var dto = Assert.IsType<JournalDTO>(ev.Payload);
        Assert.True(dto.MainQuest.HasValue);
        Assert.Equal("MainQuest", dto.MainQuest.Value!.Id);
    }

    private static State CreateState(Journal journal)
    {
        return new State { Journal = journal };
    }

    [Fact]
    public void Create_ShouldReturnJournalChangedEvent_WhenOnlyAuctionsChange()
    {
        var previousState = CreateState(CreateJournal(0, 0x00));
        var newState = CreateState(CreateJournal(0, 0x01));

        var result = JournalEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.JournalChanged, ev.Type);

        var dto = Assert.IsType<JournalDTO>(ev.Payload);
        Assert.True(dto.Auctions.HasValue);
        Assert.Equal("divineBarrier", dto.Auctions.Value![0].Id);
    }

    private static Journal CreateJournal(byte stepValue, byte auctionValue = 0)
    {
        return new Journal
        {
            MainQuest = new Quest
            {
                Id = "MainQuest",
                Steps = [new Step { Number = 1, Value = stepValue }],
                Requisites = []
            },
            SideQuests = [],
            Auctions =
            [
                new Auction { Id = "divineBarrier", Value = auctionValue },
            ],
        };
    }
}
