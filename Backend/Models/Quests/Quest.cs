namespace Backend.Models.Quests
{
    public abstract class Quest
    {
        public int Id { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Requisite> Prerequisites { get; set; } = new();
        public List<QuestStep> Steps { get; set; } = new();
    }
}
