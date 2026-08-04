using Backend.Domain.Models.Parties.Digimons;

namespace Backend.Domain.Models.Parties
{
    public record class Digimon
    {
        public int Level { get; set; }
        public int TP { get; set; }
        public int Blast { get; set; }
        public int Condition { get; set; }
        public int Experience { get; set; }
        public Vital HP { get; set; } = new();
        public Vital MP { get; set; } = new();
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
                   Blast == other.Blast &&
                   Condition == other.Condition &&
                   Experience == other.Experience &&
                   HP.Equals(other.HP) &&
                   MP.Equals(other.MP) &&
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
            hash.Add(Blast);
            hash.Add(Condition);
            hash.Add(Experience);
            hash.Add(HP);
            hash.Add(MP);
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
