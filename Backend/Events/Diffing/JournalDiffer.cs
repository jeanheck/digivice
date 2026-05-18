using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.Diffing.Journals;
using Backend.Events.DTO;

namespace Backend.Events.Diffing;

public static class JournalDiffer
{
    public static JournalDTO Diff(Journal? previousJournal, Journal newJournal)
    {
        if (newJournal.HasNoChanges(previousJournal))
        {
            return new JournalDTO();
        }

        if (previousJournal == null)
        {
            return JournalConverter.ToDTO(newJournal);
        }

        var dto = new JournalDTO();

        var mainQuestDelta = QuestDiffer.Diff(previousJournal.MainQuest, newJournal.MainQuest);
        if (mainQuestDelta != null)
        {
            dto = dto with { MainQuest = mainQuestDelta };
        }

        var sideQuestsDelta = new List<QuestDTO>();
        foreach (var sideQuestNewState in newJournal.SideQuests)
        {
            var sideQuestPreviousState = previousJournal.SideQuests.FirstOrDefault(q => q.Id == sideQuestNewState.Id);
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

        return dto;
    }
}
