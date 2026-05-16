namespace Backend.Domain.Models
{
    public record class Party
    {
        public List<Digimon?> Digimons { get; set; } = [null, null, null];

        public virtual bool Equals(Party? other)
        {
            if (other is null)
            {
                return false;
            }
            return Digimons.SequenceEqual(other.Digimons);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var slot in Digimons)
            {
                hash.Add(slot);
            }
            return hash.ToHashCode();
        }
    }
}
