using Backend.Memory.Addresses.Digimon;

namespace Backend.Memory.Addresses
{
    public class DigimonAddresses
    {
        public BasicInfoAddresses BasicInfo { get; set; } = new();
        public AttributesAddresses Attributes { get; set; } = new();
        public ResistancesAddresses Resistances { get; set; } = new();
        public EquipmentsAddresses Equipaments { get; set; } = new();
        public DigievolutionsAddresses Digievolutions { get; set; } = new();

        public void Deconstruct(
            out BasicInfoAddresses basicInfo,
            out AttributesAddresses attributes,
            out ResistancesAddresses resistances,
            out EquipmentsAddresses equipaments,
            out DigievolutionsAddresses digievolutions)
        {
            basicInfo = BasicInfo;
            attributes = Attributes;
            resistances = Resistances;
            equipaments = Equipaments;
            digievolutions = Digievolutions;
        }
    }
}
