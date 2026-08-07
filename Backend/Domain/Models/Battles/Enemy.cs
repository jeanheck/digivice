using Backend.Domain.Models.Parties.Digimons;

namespace Backend.Domain.Models.Battles
{
    public record class Enemy
    {
        public int Id { get; set; }
        public int Condition { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public Vital HP { get; set; } = new();
    }
}
