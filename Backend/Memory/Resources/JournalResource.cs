using Backend.Memory.Resources.Journal;

namespace Backend.Memory.Resources
{
    public class JournalResource
    {
        public QuestResource MainQuest { get; set; } = new();
        public List<QuestResource> SideQuests { get; set; } = [];
    }
}
