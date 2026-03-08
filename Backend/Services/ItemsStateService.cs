using Backend.Models;

namespace Backend.Services
{
    public class ItemsStateService
    {
        private readonly GameDatabase _database;
        private readonly GameReader _reader;

        public ItemsStateService(GameDatabase database, GameReader reader)
        {
            _database = database;
            _reader = reader;
        }

        public ImportantItems GetImportantItems()
        {
            var addresses = _database.GetImportantItemsAddresses();
            var resource = _reader.ReadImportantItems(addresses);

            return new ImportantItems
            {
                FolderBag = new ImportantItem { Id = addresses.FolderBag!.Id!, Name = addresses.FolderBag.Name!, Has = resource.Folderbag == 1 },
                TreeBoots = new ImportantItem { Id = addresses.TreeBoots!.Id!, Name = addresses.TreeBoots.Name!, Has = resource.TreeBoots == 1 },
                FishingPole = new ImportantItem { Id = addresses.FishingPole!.Id!, Name = addresses.FishingPole.Name!, Has = resource.FishingPole == 1 },
                RedSnapper = new ImportantItem { Id = addresses.RedSnapper!.Id!, Name = addresses.RedSnapper.Name!, Has = resource.RedSnapper == 1 }
            };
        }

        public ConsumableItems GetConsumableItems()
        {
            var addresses = _database.GetConsumableItemsAddresses();
            var resource = _reader.ReadConsumableItems(addresses);

            return new ConsumableItems
            {
                PowerCharge = new ConsumableItem { Id = addresses.PowerCharge!.Id!, Name = addresses.PowerCharge.Name!, Quantity = resource.PowerCharge },
                SpiderWeb = new ConsumableItem { Id = addresses.SpiderWeb!.Id!, Name = addresses.SpiderWeb.Name!, Quantity = resource.SpiderWeb },
                BambooSpear = new ConsumableItem { Id = addresses.BambooSpear!.Id!, Name = addresses.BambooSpear.Name!, Quantity = resource.BambooSpear }
            };
        }
    }
}
