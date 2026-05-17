using Backend.Domain.Models.Digimons;

namespace Backend.Domain.Models
{
    public record class Digimon
    {
        public int Level { get; set; }
        public int Experience { get; set; }
        public Vitals Vitals { get; set; } = new();
        public Attributes Attributes { get; set; } = new();
        public Resistances Resistances { get; set; } = new();
        public Equipments Equipments { get; set; } = new();
        public List<DigievolutionSlot> Digievolutions { get; set; } = [];
        public int ActiveDigievolutionId { get; set; }

        public virtual bool Equals(Digimon? other)
        {
            if (other is null) return false;

            return Vitals.Equals(other.Vitals) &&
                   Attributes.Equals(other.Attributes) &&
                   Resistances.Equals(other.Resistances) &&
                   Equipments.Equals(other.Equipments) &&
                   ActiveDigievolutionId == other.ActiveDigievolutionId &&
                   Digievolutions.SequenceEqual(other.Digievolutions);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Vitals);
            hash.Add(Attributes);
            hash.Add(Resistances);
            hash.Add(Equipments);
            hash.Add(ActiveDigievolutionId);
            foreach (var d in Digievolutions)
            {
                hash.Add(d);
            }
            return hash.ToHashCode();
        }
    }
}
