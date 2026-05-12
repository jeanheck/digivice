using Backend.Models.Digimons;

namespace Backend.Models
{
    public record class Digimon
    {
        public int SlotIndex { get; set; }

        public BasicInfo BasicInfo { get; set; } = new();
        public Attributes Attributes { get; set; } = new();
        public Resistances Resistances { get; set; } = new();
        public Equipments Equipments { get; set; } = new();
        public Digievolution?[] EquippedDigievolutions { get; set; } = new Digievolution?[3];
        public int? ActiveDigievolutionId { get; set; }

        public virtual bool Equals(Digimon? other)
        {
            if (other is null) return false;

            return SlotIndex == other.SlotIndex &&
                   BasicInfo.Equals(other.BasicInfo) &&
                   Attributes.Equals(other.Attributes) &&
                   Resistances.Equals(other.Resistances) &&
                   Equipments.Equals(other.Equipments) &&
                   ActiveDigievolutionId == other.ActiveDigievolutionId &&
                   EquippedDigievolutions.SequenceEqual(other.EquippedDigievolutions);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(SlotIndex);
            hash.Add(BasicInfo);
            hash.Add(Attributes);
            hash.Add(Resistances);
            hash.Add(Equipments);
            hash.Add(ActiveDigievolutionId);
            foreach (var d in EquippedDigievolutions)
            {
                hash.Add(d);
            }
            return hash.ToHashCode();
        }
    }
}
