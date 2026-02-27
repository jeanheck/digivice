using Backend.Models;

namespace Backend.Events.Data.System;

public class JournalChangedEvent : BaseEvent
{
    public Journal? Journal { get; set; }

    public JournalChangedEvent(Journal? journal) : base(EventType.JournalChanged)
    {
        Journal = journal;
    }
}
