namespace Backend.Constants
{
    internal static class MemoryAddresses
    {
        public static class Offsets
        {
            public const int PlayerName = 0x00048D88;
            public const int Bits = 0x00048DA0;

            public const int PartySlot1 = 0x00048DA4;
            public const int PartySlot2 = 0x00048DA8;
            public const int PartySlot3 = 0x00048DAC;

            public static readonly int[] PartySlots = { PartySlot1, PartySlot2, PartySlot3 };

            // Offsets relative to Digimon base address
            public const int Experience = 0x18;
            public const int Level = 0x1C;
            public const int CurrentHP = 0x20;
            public const int MaxHP = 0x22;
            public const int CurrentMP = 0x24;
            public const int MaxMP = 0x26;

            public const int Attack = 0x28;
            public const int Defense = 0x2A;
            public const int Spirit = 0x2C;
            public const int Wisdom = 0x2E;
            public const int Speed = 0x30;
            public const int Charisma = 0x32;

            public const int FireResistance = 0x34;
            public const int WaterResistance = 0x36;
            public const int IceResistance = 0x38;
            public const int WindResistance = 0x3A;
            public const int ThunderResistance = 0x3C;
            public const int MetalResistance = 0x3E;
            public const int DarkResistance = 0x40;
        }

        public static class MemoryConstants
        {
            public const int PlayerNameBufferSize = 10;
            public const int PartySlotStride = 4;
        }
    }
}
