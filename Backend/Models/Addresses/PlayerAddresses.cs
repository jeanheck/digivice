namespace Backend.Models.Addresses
{
    public class PlayerAddresses
    {
        public string Name { get; set; } = string.Empty;
        public string Bits { get; set; } = string.Empty;
        public string PartySlot1 { get; set; } = string.Empty;
        public string PartySlot2 { get; set; } = string.Empty;
        public string PartySlot3 { get; set; } = string.Empty;
        public int NameBufferSize { get; set; }
        public int PartySlotStride { get; set; }
        public string MapIdAddress { get; set; } = string.Empty;
    }
}
