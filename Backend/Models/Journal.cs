using Backend.Models.Quests;

namespace Backend.Models
{
    public class Journal
    {
        public MainQuest MainQuest { get; set; } = new MainQuest();
        public List<SideQuest> SideQuests { get; set; } = new();
    }
}
