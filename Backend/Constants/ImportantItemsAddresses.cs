namespace Backend.Constants
{
    public static class ImportantItemsAddresses
    {
        // Addreses discovered (or placeholder to be found later)
        public const int FolderBag = 0x00048F42;

        // Placeholders. We'll find their real addresses later, temporarily pointing them to FolderBag or 0 to be mapped.
        // User requested: "Na UI, vamos adicionar componentes fakes no momento para Kicking Boots, Fishing Pole e El Dorado Id"
        public const int KickingBoots = 0x00048F42; // MOCK
        public const int FishingPole = 0x00048F42; // MOCK
        public const int ElDoradoId = 0x00048F42; // MOCK

        public static readonly Dictionary<string, int> Items = new()
        {
            { "KickingBoots", KickingBoots },
            { "FishingPole", FishingPole },
            { "ElDoradoId", ElDoradoId },
            { "FolderBag", FolderBag }
        };
    }
}
