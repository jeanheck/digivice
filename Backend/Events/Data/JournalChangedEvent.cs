using Backend.Models;

namespace Backend.Events.Data;

public class JournalChangedEvent(Journal? journal) : BaseEvent(EventType.JournalChanged)
{
    public Journal? Journal { get; set; } = journal;
}
