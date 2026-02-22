using Backend.Models.Digimons;

namespace Backend.Models
{
    public class Digimon
    {
        public int SlotIndex { get; set; }

        public BasicInfo BasicInfo { get; set; } = new();
        public Attributes Attributes { get; set; } = new();
        public Resistances Resistances { get; set; } = new();
    }
}
