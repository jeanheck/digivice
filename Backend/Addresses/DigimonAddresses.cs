using System.Text.Json.Serialization;
using Backend.Utils;
using Backend.Addresses.Digimon;

namespace Backend.Addresses
{
    public class DigimonAddresses
    {
        public List<DigimonBaseAddress> Digimons { get; set; } = [];
        public BasicInfoAddresses BasicInfo { get; set; } = new();
        public AttributesAddresses Attributes { get; set; } = new();
        public ResistancesAddresses Resistances { get; set; } = new();
        public EquipmentsAddresses Equipaments { get; set; } = new();
        public DigievolutionsAddresses Digievolutions { get; set; } = new();

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int EmptySlotId { get; set; }

        public void Deconstruct(
            out List<DigimonBaseAddress> digimons,
            out BasicInfoAddresses basicInfo,
            out AttributesAddresses attributes,
            out ResistancesAddresses resistances,
            out EquipmentsAddresses equipaments,
            out DigievolutionsAddresses digievolutions)
        {
            digimons = Digimons;
            basicInfo = BasicInfo;
            attributes = Attributes;
            resistances = Resistances;
            equipaments = Equipaments;
            digievolutions = Digievolutions;
        }
    }
}
