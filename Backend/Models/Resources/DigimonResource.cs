namespace Backend.Models.Resources
{
    public class DigimonResource
    {
        public int SlotIndex { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BaseAddress { get; set; }

        // As a design decision for Phase 16 onwards: since Digimons are massive, 
        // extracting them field by field here into a naive DTO is repetitive and bloated.
        // We will store the entire 1000-byte structure block starting at BaseAddress,
        // and let DigimonStateService parse it based on the Addresses Offsets.
        public byte[] LogicBlock { get; set; } = System.Array.Empty<byte>();
        public int ActiveDigievolutionId { get; set; }
    }
}
