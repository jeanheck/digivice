using Backend.Domain.Assemblers.Journals;
using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Domain.Models.Journals.Quests;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class JournalAssembler
    {
        public static Journal Assemble(JournalResource resource)
        {
            var mainQuest = QuestAssembler.Assemble(resource.MainQuest);
            List<Quest> sideQuests = [.. resource.SideQuests.Select(QuestAssembler.Assemble)];
            List<Quest> legendaryWeapons = [.. resource.LegendaryWeapons.Select(QuestAssembler.Assemble)];
            List<Quest> driAgents = [.. resource.DriAgents.Select(QuestAssembler.Assemble)];
            List<Quest> duelIsland = [.. resource.DuelIsland.Select(QuestAssembler.Assemble)];
            List<Auction> auctions = AuctionAssembler.Assemble(resource.Auctions);
            List<Npc> npcs = NpcAssembler.Assemble(resource.Npcs);

            NormalizeMainQuestProgression(mainQuest);
            foreach (Quest duelIslandQuest in duelIsland)
            {
                NormalizeDuelIslandProgression(duelIslandQuest);
            }

            return new Journal
            {
                MainQuest = mainQuest,
                SideQuests = sideQuests,
                LegendaryWeapons = legendaryWeapons,
                DriAgents = driAgents,
                DuelIsland = duelIsland,
                Auctions = auctions,
                Npcs = npcs,
            };
        }

        private static void NormalizeMainQuestProgression(Quest mainQuest)
        {
            // Completion cascade: If the next step is completed (> 0), 
            // the current step must also be considered completed.
            for (int i = mainQuest.Steps.Count - 2; i >= 0; i--)
            {
                if (mainQuest.Steps[i].Value == 0 && mainQuest.Steps[i + 1].Value > 0)
                {
                    mainQuest.Steps[i].Value = 1;
                }
            }
        }

        private static void NormalizeDuelIslandProgression(Quest quest)
        {
            if (quest.Steps.Count == 0)
            {
                return;
            }

            Step trophyStep = quest.Steps[^1];
            if (trophyStep.Value == 0)
            {
                return;
            }

            for (int i = 0; i < quest.Steps.Count - 1; i++)
            {
                if (quest.Steps[i].Value == 0)
                {
                    quest.Steps[i].Value = 1;
                }
            }
        }
    }
}
