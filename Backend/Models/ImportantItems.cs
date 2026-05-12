namespace Backend.Models
{
    public record class ImportantItems
    {
        public ImportantItem? FolderBag { get; set; }
        public ImportantItem? TreeBoots { get; set; }
        public ImportantItem? FishingPole { get; set; }
        public ImportantItem? RedSnapper { get; set; }

        public bool HasItem(string id)
        {
            return id switch
            {
                "FolderBag" => FolderBag?.Has ?? false,
                "TreeBoots" => TreeBoots?.Has ?? false,
                "FishingPole" => FishingPole?.Has ?? false,
                "RedSnapper" => RedSnapper?.Has ?? false,
                _ => false
            };
        }
    }
}
