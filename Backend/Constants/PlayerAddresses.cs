namespace Backend.Constants
{
    internal static class PlayerAddresses
    {
        public const int Name = 0x00048D88;
        public const int Bits = 0x00048DA0;

        public const int PartySlot1 = 0x00048DA4;
        public const int PartySlot2 = 0x00048DA8;
        public const int PartySlot3 = 0x00048DAC;

        public static readonly int[] PartySlots = { PartySlot1, PartySlot2, PartySlot3 };

        public const int NameBufferSize = 10; // Used to get the name from the memory
        public const int PartySlotStride = 4; // Used to get the party slots from the memory
    }
}
