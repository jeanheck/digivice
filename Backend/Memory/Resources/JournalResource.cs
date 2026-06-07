using Backend.Memory.Resources.Journals;

namespace Backend.Memory.Resources
{
    public class JournalResource
    {
        public QuestResource MainQuest { get; set; } = new();
        public List<QuestResource> SideQuests { get; set; } = [];
        public List<QuestResource> LegendaryWeapons { get; set; } = [];
        public List<QuestResource> DriAgents { get; set; } = [];
    }
}
