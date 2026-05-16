using Backend.Memory.Resources.Quests;

namespace Backend.Memory.Resources
{
    public class QuestResource
    {
        public string Id { get; set; } = string.Empty;
        public List<RequisiteResource> Requisites { get; set; } = [];
        public List<StepResource> Steps { get; set; } = [];
    }
}
