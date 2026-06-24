namespace Backend.Domain.Models.Parties.Digimons
{
    public record class Vitals
    {
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int CurrentHP { get; set; }
        public int CurrentMP { get; set; }
    }
}
