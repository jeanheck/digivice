using Backend.Domain.Assemblers.Journals;
using Backend.Domain.Models;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class JournalAssembler
    {
        public static Journal Assemble(JournalResource resource)
        {
            return new Journal
            {
                MainQuest = MainQuestAssembler.Assemble(resource.MainQuest),
                SideQuests = [.. resource.SideQuests.Select(QuestAssembler.Assemble)],
                LegendaryWeapons = [.. resource.LegendaryWeapons.Select(QuestAssembler.Assemble)],
                DriAgents = [.. resource.DriAgents.Select(QuestAssembler.Assemble)],
                DuelIsland = [.. resource.DuelIsland.Select(DuelIslandAssembler.Assemble)],
            };
        }
    }
}
