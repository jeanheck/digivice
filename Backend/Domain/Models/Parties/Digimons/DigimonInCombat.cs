namespace Backend.Domain.Models.Parties.Digimons
{
    public record class DigimonInCombat
    {
        public int Condition { get; set; }
        public Vital HP { get; set; } = new();
        public Vital MP { get; set; } = new();
    }
}
