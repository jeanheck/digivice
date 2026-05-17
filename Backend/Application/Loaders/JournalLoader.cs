using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class JournalLoader(QuestLoader questLoader)
    {
        public JournalResource Load()
        {
            return new JournalResource
            {
                MainQuest = questLoader.LoadMainQuest(),
                SideQuests = questLoader.LoadSideQuests()
            };
        }
    }
}
