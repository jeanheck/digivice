using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Resources.Parties
{
    public class DigimonInCombatResource
    {
        public int Condition { get; set; }
        public VitalResource HP { get; set; } = new();
        public VitalResource MP { get; set; } = new();
    }
}
