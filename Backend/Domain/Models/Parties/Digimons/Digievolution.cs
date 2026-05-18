namespace Backend.Domain.Models.Parties.Digimons
{
    public record class Digievolution
    {
        public int Level { get; set; }

        public virtual bool Equals(Digievolution? other)
        {
            if (other is null)
            {
                return false;
            }
            return Level == other.Level;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Level);
            return hash.ToHashCode();
        }
    }
}
