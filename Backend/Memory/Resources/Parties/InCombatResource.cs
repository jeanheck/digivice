using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Resources.Parties
{
    public class InCombatResource
    {
        public int Condition { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public VitalResource HP { get; set; } = new();
        public VitalResource MP { get; set; } = new();
    }
}
