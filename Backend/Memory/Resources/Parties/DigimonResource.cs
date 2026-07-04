using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Resources.Parties
{
    public class DigimonResource
    {
        public int Experience { get; set; }
        public int Level { get; set; }
        public int TP { get; set; }
        public VitalsResource Vitals { get; set; } = new();
        public AttributesResource Attributes { get; set; } = new();
        public ResistancesResource Resistances { get; set; } = new();
        public EquipmentsResource Equipments { get; set; } = new();
        public List<DigievolutionSlotResource> Digievolutions { get; set; } = [];
        public List<StoredDigievolutionResource> StoredDigievolutions { get; set; } = [];
        public int ActiveDigievolutionId { get; set; }
    }
}
