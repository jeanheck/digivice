using Backend.Memory.Resources.Digimon;

namespace Backend.Memory.Resources
{
    public class DigimonResource
    {
        public int Experience { get; set; }
        public int Level { get; set; }
        public VitalsResource Vitals { get; set; } = new();
        public AttributesResource Attributes { get; set; } = new();
        public ResistancesResource Resistances { get; set; } = new();
        public EquipmentsResource Equipments { get; set; } = new();
        public List<DigievolutionSlotResource> Digievolutions { get; set; } = [];
        public int ActiveDigievolutionId { get; set; }
    }
}
