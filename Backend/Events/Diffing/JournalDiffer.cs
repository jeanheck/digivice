using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.Diffing.Journals;
using Backend.Events.DTO;
using Backend.Events.DTO.Journals;
using Backend.Events.DTO.Npcs;

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

        var sideQuestsDelta = GenerateQuestsDtos(newJournal.SideQuests, previousJournal.SideQuests);
        if (sideQuestsDelta.Count > 0)
        {
            dto = dto with { SideQuests = sideQuestsDelta };
        }

        var legendaryWeaponsDelta = GenerateQuestsDtos(newJournal.LegendaryWeapons, previousJournal.LegendaryWeapons);
        if (legendaryWeaponsDelta.Count > 0)
        {
            dto = dto with { LegendaryWeapons = legendaryWeaponsDelta };
        }

        var driAgentsDelta = GenerateQuestsDtos(newJournal.DriAgents, previousJournal.DriAgents);
        if (driAgentsDelta.Count > 0)
        {
            dto = dto with { DriAgents = driAgentsDelta };
        }

        var duelIslandDelta = GenerateQuestsDtos(newJournal.DuelIsland, previousJournal.DuelIsland);
        if (duelIslandDelta.Count > 0)
        {
            dto = dto with { DuelIsland = duelIslandDelta };
        }

        List<NpcDTO> npcsDelta = [];
        foreach (var newNpc in newJournal.Npcs)
        {
            var previousNpc = previousJournal.Npcs.FirstOrDefault(npc => npc.Id == newNpc.Id);
            var npcDelta = NpcDiffer.Diff(previousNpc, newNpc);
            if (npcDelta != null)
            {
                npcsDelta.Add(npcDelta);
            }
        }

        if (npcsDelta.Count > 0)
        {
            dto = dto with { Npcs = npcsDelta };
        }

        return dto;
    }

    private static List<QuestDTO> GenerateQuestsDtos(List<Quest> newJournalQuests, List<Quest> previousJournalQuests)
    {
        List<QuestDTO> deltas = [];
        foreach (var quest in newJournalQuests)
        {
            var sideQuestPreviousState = previousJournalQuests.FirstOrDefault(q => q.Id == quest.Id);
            var sideQuestDelta = QuestDiffer.Diff(sideQuestPreviousState, quest);
            if (sideQuestDelta != null)
            {
                deltas.Add(sideQuestDelta);
            }
        }

        return deltas;
    }
}
