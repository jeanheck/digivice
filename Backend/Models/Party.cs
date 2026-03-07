using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Models
{
    public class Party : IEquatable<Party>
    {
        public List<Digimon?> Slots { get; set; } = new() { null, null, null };
        public int ActiveSlotIndex { get; set; } = -1; // -1 significa nenhum ativo inicialmente

        public bool Equals(Party? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Slots.SequenceEqual(other.Slots);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Party)obj);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var slot in Slots)
            {
                hash.Add(slot);
            }
            return hash.ToHashCode();
        }
    }
}
