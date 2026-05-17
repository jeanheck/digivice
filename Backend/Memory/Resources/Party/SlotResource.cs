namespace Backend.Memory.Resources.Party
{
    public class SlotResource
    {
        public int Index { get; set; }
        public int DigimonId { get; set; }
        public DigimonResource Digimon { get; set; } = new();
    }
}
