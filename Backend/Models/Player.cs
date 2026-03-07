using System;

namespace Backend.Models
{
    public class Player : IEquatable<Player>
    {
        public string Name { get; set; } = string.Empty;
        public int Bits { get; set; }

        public bool Equals(Player? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Bits == other.Bits;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Player)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Bits);
        }
    }
}
