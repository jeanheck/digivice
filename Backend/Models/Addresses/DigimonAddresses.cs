namespace Backend.Models.Addresses
{
    public class DigimonBaseAddress
    {
        public string? Name { get; set; }
        public int Id { get; set; }
        public string? Address { get; set; }
    }

    public class BasicInfoOffsets
    {
        public string? Experience { get; set; }
        public string? Level { get; set; }
        public string? CurrentHP { get; set; }
        public string? MaxHP { get; set; }
        public string? CurrentMP { get; set; }
        public string? MaxMP { get; set; }
    }

    public class AttributesOffsets
    {
        public string? Strength { get; set; }
        public string? Defense { get; set; }
        public string? Spirit { get; set; }
        public string? Wisdow { get; set; }
        public string? Speed { get; set; }
        public string? Charisma { get; set; }
    }

    public class ResistancesOffsets
    {
        public string? Fire { get; set; }
        public string? Water { get; set; }
        public string? Ice { get; set; }
        public string? Wind { get; set; }
        public string? Thunder { get; set; }
        public string? Machine { get; set; }
        public string? Dark { get; set; }
    }

    public class EquipmentsOffsets
    {
        public string? Head { get; set; }
        public string? Body { get; set; }
        public string? RightHand { get; set; }
        public string? LeftHand { get; set; }
        public string? Accessory1 { get; set; }
        public string? Accessory2 { get; set; }
    }

    public class DigievolutionsOffsets
    {
        public string? ActiveDigievolution { get; set; }
        public string? EquipedSlot1 { get; set; }
        public string? EquipedSlot2 { get; set; }
        public string? EquipedSlot3 { get; set; }
        public string? UnlockedDigievolutionsStart { get; set; }
        public string? UnlockedDigievolutionEntryStride { get; set; }
        public string? MaxUnlockedDigievolutions { get; set; }
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
