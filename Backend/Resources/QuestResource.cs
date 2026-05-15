using Backend.Resources.Quests;

namespace Backend.Resources
{
    public class QuestResource
    {
        public string Id { get; set; } = string.Empty;
        public List<RequisiteResource> Requisites { get; set; } = [];
        public List<StepResource> Steps { get; set; } = [];
    }
}
