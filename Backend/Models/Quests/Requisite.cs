namespace Backend.Models.Quests
{
    public class Requisite
    {
        public string Description { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        /// <summary>
        /// Optional key to automatically check if this prerequisite is met.
        /// Maps to an entry in ConsumableItemsAddresses, ImportantItemsAddresses,
        /// or an equipment ID. When null, IsDone must be set manually.
        /// </summary>
        public string ItemKey { get; set; }
        /// <summary>
        /// Type of item to check: "consumable", "equipment", or "important".
        /// Used together with ItemKey to determine which address table to query.
        /// </summary>
        public string ItemType { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Requisite other) return false;

            return Description == other.Description &&
                   IsDone == other.IsDone &&
                   ItemKey == other.ItemKey &&
                   ItemType == other.ItemType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Description, IsDone, ItemKey, ItemType);
        }
    }
}
