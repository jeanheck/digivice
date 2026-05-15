namespace Backend.Models.Quests
{
    public record class Step
    {
        public int Number { get; set; }
        public byte Value { get; set; }
        public List<Requisite> Requisites { get; set; } = [];

        public virtual bool Equals(Step? other)
        {
            if (other is null) return false;

            bool requisitesEqual = (Requisites == null && other.Requisites == null) ||
                                 (Requisites != null && other.Requisites != null &&
                                  Requisites.SequenceEqual(other.Requisites));

            return Number == other.Number &&
                   Value == other.Value &&
                   requisitesEqual;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Number);
            hash.Add(Value);
            if (Requisites != null)
            {
                foreach (var r in Requisites) hash.Add(r);
            }
            return hash.ToHashCode();
        }
    }
}
