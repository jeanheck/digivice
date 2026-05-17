namespace Backend.Memory.Resources.Digimon
{
    public class DigievolutionSlotResource
    {
        public int Index { get; set; }
        public int DigievolutionId { get; set; }
        public DigievolutionResource DigievolutionResource { get; set; } = new();
    }
}
