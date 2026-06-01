namespace Backend.Domain.Models.Parties
{
    public record class DigimonSlot
    {
        public int Index { get; set; }
        public int? DigimonId { get; set; }
        public Digimon? Digimon { get; set; }
    }
}
