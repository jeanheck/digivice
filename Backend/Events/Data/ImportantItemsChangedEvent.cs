using Backend.Models;

namespace Backend.Events.Data;

public class ImportantItemsChangedEvent(ImportantItems importantItems) : BaseEvent(EventType.ImportantItemsChanged)
{
    public ImportantItems ImportantItems { get; set; } = importantItems;
}
