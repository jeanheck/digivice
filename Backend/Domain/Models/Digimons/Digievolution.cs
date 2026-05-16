namespace Backend.Domain.Models.Digimons
{
    public record class Digievolution
    {
        public int Id { get; set; }
        public int Level { get; set; }

        public virtual bool Equals(Digievolution? other)
        {
            if (other is null) return false;
            return Id == other.Id && 
                   Level == other.Level;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Id);
            hash.Add(Level);
            return hash.ToHashCode();
        }
    }
}
