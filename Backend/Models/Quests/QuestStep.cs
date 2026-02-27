namespace Backend.Models.Quests
{
    public class QuestStep
    {
        public int Number { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
