namespace Backend.Models.Quests
{
    public class QuestStep
    {
        public int Number { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        /// <summary>
        /// Optional informational prerequisites for this step.
        /// Displayed as a checklist inside the step — does NOT block progress.
        /// </summary>
        public List<Requisite>? Prerequisites { get; set; }
    }
}
