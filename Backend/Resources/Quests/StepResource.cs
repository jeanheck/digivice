namespace Backend.Resources.Quests
{
    public class StepResource
    {
        public int Number { get; set; }
        public byte Value { get; set; }
        public List<RequisiteResource> Requisites { get; set; } = [];
    }
}
