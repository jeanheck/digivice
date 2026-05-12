namespace Backend.Models.Quests
{
    public record class Requisite
    {
        public string Description { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        /// <summary>
        /// Optional key to automatically check if this prerequisite is met.
        /// Maps to an entry in ConsumableItemsAddresses, ImportantItemsAddresses,
        /// or an equipment ID. When null, IsDone must be set manually.
        /// </summary>
        public string ItemKey { get; set; } = string.Empty;
        /// <summary>
        /// Type of item to check: "consumable", "equipment", or "important".
        /// Used together with ItemKey to determine which address table to query.
        /// </summary>
        public string ItemType { get; set; } = string.Empty;
    }
}
