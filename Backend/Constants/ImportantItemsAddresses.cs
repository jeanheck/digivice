namespace Backend.Constants
{
    public static class ImportantItemsAddresses
    {
        // Discovered addresses
        public const int FolderBag = 0x00048F42;
        public const int TreeBoots = 0x00048DB4;
        public const int FishingPole = 0x00048DB5;
        public const int RedSnapper = 0x00048DB7;

        // Placeholders — real addresses to be discovered later
        public const int ElDoradoId = 0x00048F42; // MOCK

        public static readonly Dictionary<string, int> Items = new()
        {
            { "TreeBoots", TreeBoots },
            { "FishingPole", FishingPole },
            { "RedSnapper", RedSnapper },
            { "ElDoradoId", ElDoradoId },
            { "FolderBag", FolderBag }
        };
    }
}
