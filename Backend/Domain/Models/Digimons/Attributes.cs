namespace Backend.Domain.Models.Digimons
{
    public record class Attributes
    {
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Spirit { get; set; }
        public int Wisdom { get; set; }
        public int Speed { get; set; }
        public int Charisma { get; set; }
    }
}
