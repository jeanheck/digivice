using Backend.Memory.Resources.Journal.Quest;

namespace Backend.Memory.Resources.Journal
{
    public class QuestResource
    {
        public string Id { get; set; } = string.Empty;
        public List<RequisiteResource> Requisites { get; set; } = [];
        public List<StepResource> Steps { get; set; } = [];
    }
}
