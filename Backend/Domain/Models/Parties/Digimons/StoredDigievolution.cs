namespace Backend.Domain.Models.Parties.Digimons
{
    public record class StoredDigievolution
    {
        public int DigievolutionId { get; set; }
        public int Level { get; set; }

        public virtual bool Equals(StoredDigievolution? other)
        {
            if (other is null)
            {
                return false;
            }

            return DigievolutionId == other.DigievolutionId &&
                   Level == other.Level;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(DigievolutionId);
            hash.Add(Level);
            return hash.ToHashCode();
        }
    }
}
