using System;

namespace Backend.Models
{
    public class ImportantItems : IEquatable<ImportantItems>
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

        public bool Equals(ImportantItems? other)
        {
            if (other is null) return false;

            bool EqualOrNull<T>(T? a, T? b) where T : IEquatable<T>
            {
                if (a == null && b == null) return true;
                if (a == null || b == null) return false;
                return a.Equals(b);
            }

            return EqualOrNull(FolderBag, other.FolderBag) &&
                   EqualOrNull(TreeBoots, other.TreeBoots) &&
                   EqualOrNull(FishingPole, other.FishingPole) &&
                   EqualOrNull(RedSnapper, other.RedSnapper);
        }

        public override bool Equals(object? obj) => Equals(obj as ImportantItems);
        public override int GetHashCode() => HashCode.Combine(FolderBag, TreeBoots, FishingPole, RedSnapper);
    }
}
