using System;

namespace Backend.Models
{
    public class ConsumableItem : IItem, IEquatable<ConsumableItem>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }

        public bool Equals(ConsumableItem? other)
        {
            if (other is null) return false;
            return Id == other.Id && Name == other.Name && Quantity == other.Quantity;
        }

        public override bool Equals(object? obj) => Equals(obj as ConsumableItem);
        public override int GetHashCode() => HashCode.Combine(Id, Name, Quantity);
    }
}
