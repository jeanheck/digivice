using Backend.Domain.Models;
using Backend.Events.Converters;
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
                    MainQuest = QuestConverter.ToDTO(newState.MainQuest),
                    SideQuests = newState.SideQuests.Select(QuestConverter.ToDTO).ToList()
                })
            ];
        }

        var dto = new JournalDTO();

        var mainQuestDelta = QuestDiffer.Diff(previous.MainQuest, newState.MainQuest);
        if (mainQuestDelta != null)
        {
            dto = dto with { MainQuest = mainQuestDelta };
        }

        var sideQuestsDelta = new List<QuestDTO>();
        foreach (var sideQuestNewState in newState.SideQuests)
        {
            var sideQuestPreviousState = previous.SideQuests.FirstOrDefault(q => q.Id == sideQuestNewState.Id);
            var sideQuestDelta = QuestDiffer.Diff(sideQuestPreviousState, sideQuestNewState);
            if (sideQuestDelta != null)
            {
                sideQuestsDelta.Add(sideQuestDelta);
            }
        }

        if (sideQuestsDelta.Count > 0)
        {
            dto = dto with { SideQuests = sideQuestsDelta };
        }

        if (mainQuestDelta != null || sideQuestsDelta.Count > 0)
        {
            return [new BaseEvent(JournalEvent.JournalChanged, dto)];
        }

        return [];
    }
}
