using Backend.Memory.Addresses.Journal.Quests;

namespace Backend.Memory.Addresses.Journal
{
    public class QuestAddresses
    {
        public string Id { get; set; } = string.Empty;
        public List<RequisiteAddresses> Requisites { get; set; } = [];
        public List<StepAddresses> Steps { get; set; } = [];
    }
}
