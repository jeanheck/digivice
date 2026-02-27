namespace Backend.Events.Data.System;

public class ImportantItemsChangedEvent : BaseEvent
{
    public Dictionary<string, bool> ImportantItems { get; set; }

    public ImportantItemsChangedEvent(Dictionary<string, bool> importantItems) : base(EventType.ImportantItemsChanged)
    {
        ImportantItems = importantItems;
    }
}
