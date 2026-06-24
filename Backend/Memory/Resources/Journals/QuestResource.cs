using Backend.Memory.Resources.Journals.Quests;

namespace Backend.Memory.Resources.Journals
{
    public class QuestResource
    {
        public string Id { get; set; } = string.Empty;
        public List<RequisiteResource> Requisites { get; set; } = [];
        public List<StepResource> Steps { get; set; } = [];
    }
}
