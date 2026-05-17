namespace Backend.Memory.Resources.Party
{
    public class DigimonSlotResource
    {
        public int Index { get; set; }
        public int? DigimonId { get; set; }
        public DigimonResource DigimonResource { get; set; } = new();
    }
}
