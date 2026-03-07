using System.Collections.Generic;
using System.Linq;

namespace Backend.Services
{
    public class ItemStateService
    {
        private readonly GameDatabase _database;
        private readonly GameReader _reader;

        public ItemStateService(GameDatabase database, GameReader reader)
        {
            _database = database;
            _reader = reader;
        }

        public Dictionary<string, bool> GetImportantItems()
        {
            var addresses = _database.GetImportantItemsAddresses();
            var resource = _reader.ReadImportantItems(addresses);

            // Important items are boolean flags (0 or 1). We convert byte == 1 to True.
            return new Dictionary<string, bool>
            {
                { "Folderbag", resource.Folderbag == 1 },
                { "TreeBoots", resource.TreeBoots == 1 },
                { "FishingPole", resource.FishingPole == 1 },
                { "RedSnapper", resource.RedSnapper == 1 }
            };
        }

        public Dictionary<string, int> GetConsumableItems()
        {
            var addresses = _database.GetConsumableItemsAddresses();
            var resource = _reader.ReadConsumableItems(addresses);

            // Consumable items map raw byte quantities to integers
            return new Dictionary<string, int>
            {
                { "PowerCharge", resource.PowerCharge },
                { "SpiderWeb", resource.SpiderWeb },
                { "BambooSpear", resource.BambooSpear }
            };
        }
    }
}
