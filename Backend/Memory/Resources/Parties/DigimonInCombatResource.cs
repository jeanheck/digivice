using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Resources.Parties
{
    public class DigimonInCombatResource
    {
        public const int CombatMapId = 0x0600;

        public int MapId { get; set; }
        public int Id { get; set; }
        public int Condition { get; set; }
        public VitalResource HP { get; set; } = new();
        public VitalResource MP { get; set; } = new();

        public bool IsCombatMap => MapId == CombatMapId;

        public bool IsInCombat => IsCombatMap && HP.Max != 0;
    }
}
