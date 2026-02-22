namespace Backend.Models.Digimons
{
    public class Digimon
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BaseAddress { get; set; }
        public int SlotIndex { get; set; }

        public int Level { get; set; }
        public int Experience { get; set; }

        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int CurrentHP { get; set; }
        public int CurrentMP { get; set; }

        public Attributes Attributes { get; set; } = new();
        public Resistances Resistances { get; set; } = new();
    }
}
