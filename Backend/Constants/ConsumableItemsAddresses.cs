namespace Backend.Constants
{
    /// <summary>
    /// RAM addresses for consumable/quest item quantities.
    /// Each address holds a single byte representing the quantity owned.
    /// Pattern: same as ImportantItems (fixed address per item, byte = qty).
    /// Save→RAM mapping: RAM = save_offset + 0x46A34
    /// </summary>
    public static class ConsumableItemsAddresses
    {
        // Discovered via memory card save comparison
        public const int PowerCharge = 0x048DDB;   // save 0x0023A7
        public const int SpiderWeb = 0x048E09;     // save 0x0023D5
        public const int BambooSpear = 0x048E57;   // RAM snapshot diff — weapon ownership

        public static readonly Dictionary<string, int> Items = new()
        {
            { "PowerCharge", PowerCharge },
            { "SpiderWeb", SpiderWeb },
            { "BambooSpear", BambooSpear }
        };
    }
}
