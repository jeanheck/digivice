using System.Collections.Generic;

namespace Backend.Models.Quests
{
    public abstract class Quest
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Requirements { get; set; } = new();
        public List<QuestStep> Steps { get; set; } = new();
        public bool Done { get; set; }
        public bool Available { get; set; }
    }
}
