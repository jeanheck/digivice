namespace Backend.Domain.Backend.Domain.Models.Digimons
{
    public record class Digievolution
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public List<Technique> Techniques { get; set; } = [];

        public virtual bool Equals(Digievolution? other)
        {
            if (other is null) return false;
            return Id == other.Id && 
                   Level == other.Level && 
                   Techniques.SequenceEqual(other.Techniques);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Id);
            hash.Add(Level);
            foreach (var tech in Techniques)
            {
                hash.Add(tech);
            }
            return hash.ToHashCode();
        }
    }
}
