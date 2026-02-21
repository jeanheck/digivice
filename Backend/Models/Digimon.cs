namespace Backend.Models
{
    public class Digimon
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BaseAddress { get; set; }

        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int CurrentHP { get; set; }
        public int CurrentMP { get; set; }

        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Spirit { get; set; }
        public int Wisdom { get; set; }
        public int Speed { get; set; }
        public int Charisma { get; set; }

        public int FireResistance { get; set; }
        public int WaterResistance { get; set; }
        public int IceResistance { get; set; }
        public int WindResistance { get; set; }
        public int ThunderResistance { get; set; }
        public int MetalResistance { get; set; }
        public int DarkResistance { get; set; }
    }
}
