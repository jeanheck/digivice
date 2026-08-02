using System.Text.Json.Serialization;
using Backend.Memory.Converters;
using Backend.Memory.Addresses.Parties.Digimons;

namespace Backend.Memory.Addresses.Parties
{
    public class DigimonStatusAddresses
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Experience { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Level { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int TP { get; set; }
        public VitalAddresses HP { get; set; } = new();
        public VitalAddresses MP { get; set; } = new();
        public AttributesAddresses Attributes { get; set; } = new();
        public ResistancesAddresses Resistances { get; set; } = new();
        public EquipmentsAddresses Equipaments { get; set; } = new();
        public DigievolutionsAddresses Digievolutions { get; set; } = new();

        public void Deconstruct(
            out int experience,
            out int level,
            out int tp,
            out VitalAddresses hp,
            out VitalAddresses mp,
            out AttributesAddresses attributes,
            out ResistancesAddresses resistances,
            out EquipmentsAddresses equipaments,
            out DigievolutionsAddresses digievolutions)
        {
            experience = Experience;
            level = Level;
            tp = TP;
            hp = HP;
            mp = MP;
            attributes = Attributes;
            resistances = Resistances;
            equipaments = Equipaments;
            digievolutions = Digievolutions;
        }
    }
}
