using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Models.Addresses
{
    public class DigimonBaseAddress
    {
        public string? Name { get; set; }
        public int Id { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Address { get; set; }
    }

    public class BasicInfoOffsets
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Experience { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Level { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int CurrentHP { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int MaxHP { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int CurrentMP { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int MaxMP { get; set; }
    }

    public class AttributesOffsets
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Strength { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Defense { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Spirit { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Wisdow { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Speed { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Charisma { get; set; }
    }

    public class ResistancesOffsets
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Fire { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Water { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Ice { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Wind { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Thunder { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Machine { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Dark { get; set; }
    }

    public class EquipmentsOffsets
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Head { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Body { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int RightHand { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int LeftHand { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Accessory1 { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Accessory2 { get; set; }
    }

    public class DigievolutionsOffsets
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int ActiveDigievolution { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int EquipedSlot1 { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int EquipedSlot2 { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int EquipedSlot3 { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int UnlockedDigievolutionsStart { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int UnlockedDigievolutionEntryStride { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int MaxUnlockedDigievolutions { get; set; }
    }

    public class DigimonAddresses
    {
        public List<DigimonBaseAddress>? Digimons { get; set; }

        public BasicInfoOffsets? BasicInfo { get; set; }
        public AttributesOffsets? Attributes { get; set; }
        public ResistancesOffsets? Resistances { get; set; }
        public EquipmentsOffsets? Equipaments { get; set; }
        public DigievolutionsOffsets? Digievolutions { get; set; }

        public string? EmptySlotId { get; set; }
    }
}
