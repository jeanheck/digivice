namespace Backend.Domain.Models
{
    public class Slot
    {
        public int Index { get; set; }
        public int DigimonId { get; set; }
        public Digimon? Digimon { get; set; }
    }
}
