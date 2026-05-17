namespace Backend.Memory.Resources.Journal.Quest
{
    public class StepResource
    {
        public int Number { get; set; }
        public byte Value { get; set; }
        public List<RequisiteResource> Requisites { get; set; } = [];
    }
}
