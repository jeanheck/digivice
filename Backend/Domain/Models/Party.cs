namespace Backend.Domain.Models
{
    public record class Party
    {
        public List<Digimon?> Slots { get; set; } = [null, null, null];

        public virtual bool Equals(Party? other)
        {
            if (other is null)
            {
                return false;
            }
            return Slots.SequenceEqual(other.Slots);
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
