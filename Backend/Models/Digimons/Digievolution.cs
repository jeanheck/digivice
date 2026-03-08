namespace Backend.Models.Digimons
{
    public class Digievolution : IEquatable<Digievolution>
    {
        public int Id { get; set; }
        public int Level { get; set; }

        public bool Equals(Digievolution? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Level == other.Level;
        }

        public override bool Equals(object? obj) => Equals(obj as Digievolution);
        public override int GetHashCode() => HashCode.Combine(Id, Level);
        public List<Technique> Techniques { get; set; } = new();
    }
}
