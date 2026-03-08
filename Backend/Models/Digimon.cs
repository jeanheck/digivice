using Backend.Models.Digimons;

namespace Backend.Models
{
    public class Digimon : IEquatable<Digimon>
    {
        public int SlotIndex { get; set; }

        public BasicInfo BasicInfo { get; set; } = new();
        public Attributes Attributes { get; set; } = new();
        public Resistances Resistances { get; set; } = new();
        public Equipments Equipments { get; set; } = new();
        public Digievolution?[] EquippedDigievolutions { get; set; } = new Digievolution?[3];

        public bool Equals(Digimon? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (SlotIndex != other.SlotIndex ||
                !BasicInfo.Equals(other.BasicInfo) ||
                !Attributes.Equals(other.Attributes) ||
                !Resistances.Equals(other.Resistances) ||
                !Equipments.Equals(other.Equipments)) return false;

            for (int i = 0; i < 3; i++)
            {
                var a = EquippedDigievolutions[i];
                var b = other.EquippedDigievolutions[i];
                if (a == null && b != null) return false;
                if (a != null && b == null) return false;
                if (a != null && b != null && !a.Equals(b)) return false;
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Digimon)obj);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(SlotIndex);
            hash.Add(BasicInfo);
            hash.Add(Attributes);
            hash.Add(Resistances);
            hash.Add(Equipments);
            foreach (var d in EquippedDigievolutions)
            {
                hash.Add(d);
            }
            return hash.ToHashCode();
        }
    }
}
