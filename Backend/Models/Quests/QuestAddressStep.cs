namespace Backend.Models.Quests
{
    public class QuestAddressStep
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Bitmask to check against the byte at Address.
        /// Step is completed if (byte AND BitMask) != 0.
        /// 
        /// For cumulative flags: use a single bit (e.g., "0x04" for bit 2).
        /// For rotating/phase flags: use multiple bits (e.g., "0xE0" for bits 5+6+7,
        ///   meaning "step is done if ANY of these bits is set").
        /// 
        /// If null, uses legacy behavior: byte == 1 means completed.
        /// </summary>
        public string? BitMask { get; set; }
    }
}
