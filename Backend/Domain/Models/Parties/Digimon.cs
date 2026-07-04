using Backend.Domain.Models.Parties.Digimons;

namespace Backend.Domain.Models.Parties
{
    public record class Digimon
    {
        public int Level { get; set; }
        public int TP { get; set; }
        public int Experience { get; set; }
        public Vitals Vitals { get; set; } = new();
        public Attributes Attributes { get; set; } = new();
        public Resistances Resistances { get; set; } = new();
        public Equipments Equipments { get; set; } = new();
        public List<DigievolutionSlot> Digievolutions { get; set; } = [];
        public List<StoredDigievolution> StoredDigievolutions { get; set; } = [];
        public int ActiveDigievolutionId { get; set; }

        public virtual bool Equals(Digimon? other)
        {
            if (other is null) return false;

            return Level == other.Level &&
                   TP == other.TP &&
                   Experience == other.Experience &&
                   Vitals.Equals(other.Vitals) &&
                   Attributes.Equals(other.Attributes) &&
                   Resistances.Equals(other.Resistances) &&
                   Equipments.Equals(other.Equipments) &&
                   ActiveDigievolutionId == other.ActiveDigievolutionId &&
                   Digievolutions.SequenceEqual(other.Digievolutions) &&
                   StoredDigievolutions.SequenceEqual(other.StoredDigievolutions);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Level);
            hash.Add(TP);
            hash.Add(Experience);
            hash.Add(Vitals);
            hash.Add(Attributes);
            hash.Add(Resistances);
            hash.Add(Equipments);
            hash.Add(ActiveDigievolutionId);
            foreach (var digievolutionSlot in Digievolutions)
            {
                hash.Add(digievolutionSlot);
            }
            foreach (var storedDigievolution in StoredDigievolutions)
            {
                hash.Add(storedDigievolution);
            }
            return hash.ToHashCode();
        }
    }
}
