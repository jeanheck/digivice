namespace Backend.Models
{
    public record class ImportantItems
    {
        public ImportantItem FolderBag { get; set; } = new();
        public ImportantItem TreeBoots { get; set; } = new();
        public ImportantItem FishingPole { get; set; } = new();
        public ImportantItem RedSnapper { get; set; } = new();

        public bool HasItem(string id)
        {
            return id switch
            {
                "FolderBag" => FolderBag.Has,
                "TreeBoots" => TreeBoots.Has,
                "FishingPole" => FishingPole.Has,
                "RedSnapper" => RedSnapper.Has,
                _ => false
            };
        }
    }
}
