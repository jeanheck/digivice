namespace Backend.Events.Models.Journal;

public class JournalChangedEvent(Backend.Domain.Models.Journal? journal) : BaseEvent(EventType.JournalChanged)
{
    public Backend.Domain.Models.Journal? Journal { get; set; } = journal;
}
