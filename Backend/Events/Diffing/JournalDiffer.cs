using Backend.Domain.Models;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO;
using Backend.Events.Models;
using Backend.Events.Types;

namespace Backend.Events.Diffing;

public static class JournalDiffer
{
    public static IEnumerable<BaseEvent> Diff(Journal? previous, Journal? newState)
    {
        if (newState.HasNoChanges(previous))
        {
            return [];
        }

        if (previous == null)
        {
            return [
                new BaseEvent(JournalEvent.JournalChanged, new JournalDTO
                {
                    MainQuest = newState.MainQuest,
                    SideQuests = newState.SideQuests
                })
            ];
        }

        var dto = new JournalDTO();

        if (newState.MainQuest != previous.MainQuest)
        {
            dto = dto with { MainQuest = newState.MainQuest };
        }

        bool sideQuestsChanged = (newState.SideQuests == null && previous.SideQuests != null) ||
                                 (newState.SideQuests != null && previous.SideQuests == null) ||
                                 (newState.SideQuests != null && previous.SideQuests != null &&
                                  !newState.SideQuests.SequenceEqual(previous.SideQuests));

        if (sideQuestsChanged)
        {
            dto = dto with { SideQuests = newState.SideQuests };
        }

        return [new BaseEvent(JournalEvent.JournalChanged, dto)];
    }
}
