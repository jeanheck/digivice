using Backend.Models;

namespace Backend.Events.Data.System;

public class ImportantItemsChangedEvent : BaseEvent
{
    public ImportantItems? ImportantItems { get; set; }

    public ImportantItemsChangedEvent(ImportantItems? importantItems) : base(EventType.ImportantItemsChanged)
    {
        ImportantItems = importantItems;
    }
}
