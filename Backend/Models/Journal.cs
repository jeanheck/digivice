using System.Collections.Generic;
using Backend.Models.Quests;

namespace Backend.Models
{
    public class Journal
    {
        public MainQuest? MainQuest { get; set; }
        public List<SideQuest> SideQuests { get; set; } = new();
    }
}
