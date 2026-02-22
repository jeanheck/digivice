namespace Backend.Models.Digimons
{
    public class Digimon
    {
        public string Name { get; set; } = string.Empty;
        public int SlotIndex { get; set; }

        public BasicInfo BasicInfo { get; set; } = new();
        public Attributes Attributes { get; set; } = new();
        public Resistances Resistances { get; set; } = new();
    }
}
