namespace Backend.Events.Models.Journal;

public class JournalChangedEvent(Domain.Models.Journal? journal) : BaseEvent(EventType.JournalChanged)
{
    public Domain.Models.Journal? Journal { get; set; } = journal;
}
