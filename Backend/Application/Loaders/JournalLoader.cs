using Backend.Application.Loaders.Journals;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class JournalLoader(QuestLoader questLoader, IAuctionLoader auctionLoader) : IJournalLoader
    {
        public JournalResource Load()
        {
            return new JournalResource
            {
                MainQuest = questLoader.LoadMainQuest(),
                SideQuests = questLoader.LoadSideQuests(),
                LegendaryWeapons = questLoader.LoadLegendaryWeapons(),
                DriAgents = questLoader.LoadDriAgents(),
                Auctions = auctionLoader.LoadAuctions(),
            };
        }
    }
}
