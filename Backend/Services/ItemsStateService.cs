using Backend.Models;
using Backend.Interfaces;
using Backend.Addresses;

namespace Backend.Services
{
    public class ItemsStateService(IGameDatabase gameDatabase, IGameReader gameReader)
    {
        public ImportantItems GetImportantItems()
        {
            var addresses = gameDatabase.GetImportantItemsAddresses();
            var resource = gameReader.ReadImportantItems(addresses);

            return new ImportantItems
            {
                FolderBag = CreateImportantItem(addresses.FolderBag, resource.FolderBag),
                TreeBoots = CreateImportantItem(addresses.TreeBoots, resource.TreeBoots),
                FishingPole = CreateImportantItem(addresses.FishingPole, resource.FishingPole),
                RedSnapper = CreateImportantItem(addresses.RedSnapper, resource.RedSnapper)
            };
        }

        public ConsumableItems GetConsumableItems()
        {
            var addresses = gameDatabase.GetConsumableItemsAddresses();
            var resource = gameReader.ReadConsumableItems(addresses);

            return new ConsumableItems
            {
                PowerCharge = CreateConsumableItem(addresses.PowerCharge, resource.PowerCharge),
                SpiderWeb = CreateConsumableItem(addresses.SpiderWeb, resource.SpiderWeb),
                BambooSpear = CreateConsumableItem(addresses.BambooSpear, resource.BambooSpear)
            };
        }

        private static ImportantItem CreateImportantItem(ItemAddress address, byte resourceValue)
        {
            return new ImportantItem { Id = address.Id, Name = address.Name, Has = resourceValue != 0 };
        }

        private static ConsumableItem CreateConsumableItem(ItemAddress address, byte resourceValue)
        {
            return new ConsumableItem { Id = address.Id, Name = address.Name, Quantity = resourceValue };
        }
    }
}
