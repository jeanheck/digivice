using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Parties
{
    public class InCombat
    {
        public int Id { get; set; }
        public VitalResource HP { get; set; } = new();
        public VitalResource MP { get; set; } = new();
    }
}
