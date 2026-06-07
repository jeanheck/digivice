using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.Diffing.Journals;
using Backend.Events.DTO;
using Backend.Events.DTO.Journals;

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

        var legendaryWeaponsDelta = new List<QuestDTO>();
        foreach (var legendaryWeaponNewState in newJournal.LegendaryWeapons)
        {
            var legendaryWeaponPreviousState = previousJournal.LegendaryWeapons
                .FirstOrDefault(quest => quest.Id == legendaryWeaponNewState.Id);
            var legendaryWeaponDelta = QuestDiffer.Diff(legendaryWeaponPreviousState, legendaryWeaponNewState);
            if (legendaryWeaponDelta != null)
            {
                legendaryWeaponsDelta.Add(legendaryWeaponDelta);
            }
        }

        if (legendaryWeaponsDelta.Count > 0)
        {
            dto = dto with { LegendaryWeapons = legendaryWeaponsDelta };
        }

        var driAgentsDelta = new List<QuestDTO>();
        foreach (var driAgentNewState in newJournal.DriAgents)
        {
            var driAgentPreviousState = previousJournal.DriAgents
                .FirstOrDefault(quest => quest.Id == driAgentNewState.Id);
            var driAgentDelta = QuestDiffer.Diff(driAgentPreviousState, driAgentNewState);
            if (driAgentDelta != null)
            {
                driAgentsDelta.Add(driAgentDelta);
            }
        }

        if (driAgentsDelta.Count > 0)
        {
            dto = dto with { DriAgents = driAgentsDelta };
        }

        return dto;
    }
}
