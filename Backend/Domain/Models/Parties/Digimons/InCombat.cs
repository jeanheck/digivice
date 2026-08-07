namespace Backend.Domain.Models.Parties.Digimons
{
    public record class InCombat
    {
        public int Condition { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public Vital HP { get; set; } = new();
        public Vital MP { get; set; } = new();
    }
}
