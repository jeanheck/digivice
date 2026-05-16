using Backend.Domain.Models;
using Backend.Events.Models;
using Backend.Events.Models.Journal;

namespace Backend.Events.Diffing;

public static class JournalDiffer
{
    public static IEnumerable<BaseEvent> Diff(Journal? previous, Journal? current)
    {
        var events = new List<BaseEvent>();

        bool hasChanged = (previous == null && current != null) ||
                          (previous != null && current == null) ||
                          (previous != null && current != null && !previous.Equals(current));

        if (hasChanged)
        {
            events.Add(new JournalChangedEvent(current));
        }

        return events;
    }
}
