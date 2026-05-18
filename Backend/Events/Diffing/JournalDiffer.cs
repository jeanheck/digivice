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

        // 1. Estado Inicial Completo
        if (previous == null)
        {
            var initialMain = QuestDiffer.Diff(null, newState.MainQuest)!;
            var initialSides = newState.SideQuests.Select(q => QuestDiffer.Diff(null, q)!).ToList();

            return [
                new BaseEvent(JournalEvent.JournalChanged, new JournalDTO
                {
                    MainQuest = initialMain,
                    SideQuests = initialSides
                })
            ];
        }

        var dto = new JournalDTO();

        // 2. Compara a missão principal recursivamente
        var mainQuestDelta = QuestDiffer.Diff(previous.MainQuest, newState.MainQuest);
        if (mainQuestDelta != null)
        {
            dto = dto with { MainQuest = mainQuestDelta };
        }

        // 3. Compara as missões secundárias
        var sideQuestsDelta = new List<QuestDTO>();

        foreach (var currSide in newState.SideQuests)
        {
            var prevSide = previous.SideQuests.FirstOrDefault(q => q.Id == currSide.Id);
            var sideDelta = QuestDiffer.Diff(prevSide, currSide);
            if (sideDelta != null)
            {
                sideQuestsDelta.Add(sideDelta);
            }
        }

        if (sideQuestsDelta.Count > 0)
        {
            dto = dto with { SideQuests = sideQuestsDelta };
        }

        // 4. Retorna evento apenas se houver pelo menos um delta real detectado
        if (mainQuestDelta != null || sideQuestsDelta.Count > 0)
        {
            return [new BaseEvent(JournalEvent.JournalChanged, dto)];
        }

        return [];
    }
}
