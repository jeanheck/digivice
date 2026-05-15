using Backend.Addresses.Quests;

namespace Backend.Addresses
{
    public class QuestAddresses
    {
        public string Id { get; set; } = string.Empty;
        public List<RequisiteAddresses> Requisites { get; set; } = [];
        public List<StepAddresses> Steps { get; set; } = [];
    }
}
