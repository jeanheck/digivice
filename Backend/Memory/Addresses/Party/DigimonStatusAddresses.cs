using System.Text.Json.Serialization;
using Backend.Memory.Converters;
using Backend.Memory.Addresses.Party.Digimon;

namespace Backend.Memory.Addresses.Party
{
    public class DigimonStatusAddresses
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Experience { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Level { get; set; }
        public VitalsAddresses Vitals { get; set; } = new();
        public AttributesAddresses Attributes { get; set; } = new();
        public ResistancesAddresses Resistances { get; set; } = new();
        public EquipmentsAddresses Equipaments { get; set; } = new();
        public DigievolutionsAddresses Digievolutions { get; set; } = new();

        public void Deconstruct(
            out int experience,
            out int level,
            out VitalsAddresses vitals,
            out AttributesAddresses attributes,
            out ResistancesAddresses resistances,
            out EquipmentsAddresses equipaments,
            out DigievolutionsAddresses digievolutions)
        {
            experience = Experience;
            level = Level;
            vitals = Vitals;
            attributes = Attributes;
            resistances = Resistances;
            equipaments = Equipaments;
            digievolutions = Digievolutions;
        }
    }
}
