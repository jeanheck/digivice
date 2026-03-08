using System;

namespace Backend.Models
{
    public class ImportantItem : IItem, IEquatable<ImportantItem>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool Has { get; set; }

        public bool Equals(ImportantItem? other)
        {
            if (other is null) return false;
            return Id == other.Id && Name == other.Name && Has == other.Has;
        }

        public override bool Equals(object? obj) => Equals(obj as ImportantItem);
        public override int GetHashCode() => HashCode.Combine(Id, Name, Has);
    }
}
