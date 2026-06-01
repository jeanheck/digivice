namespace Backend.Domain.Models.Parties.Digimons
{
    public record class DigievolutionSlot
    {
        public int Index { get; set; }
        public int DigievolutionId { get; set; }
        public Digievolution? Digievolution { get; set; }
    }
}
