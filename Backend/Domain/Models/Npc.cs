namespace Backend.Domain.Models
{
    public record class Npc
    {
        public List<NpcBattle> Battles { get; set; } = [];

        public virtual bool Equals(Npc? other)
        {
            if (other is null)
            {
                return false;
            }

            bool battlesEqual = (Battles == null && other.Battles == null) ||
                (Battles != null && other.Battles != null &&
                 Battles.SequenceEqual(other.Battles));

            return battlesEqual;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            if (Battles != null)
            {
                foreach (var battle in Battles)
                {
                    hash.Add(battle);
                }
            }
            return hash.ToHashCode();
        }
    }
}
