using Backend.Application.Loaders.Journals;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class JournalLoader(IQuestLoader questLoader) : IJournalLoader
    {
        public JournalResource Load()
        {
            return new JournalResource
            {
                MainQuest = questLoader.LoadMainQuest(),
                SideQuests = questLoader.LoadSideQuests(),
                LegendaryWeapons = questLoader.LoadLegendaryWeapons(),
                DriAgents = questLoader.LoadDriAgents(),
                DuelIsland = questLoader.LoadDuelIsland(),
            };
        }
    }
}
