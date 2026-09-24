namespace Backend.Domain.Models.Journals.Quests
{
    public record class Step
    {
        public int Number { get; set; }
        public bool IsDone { get; set; }
        public List<Requisite> Requisites { get; set; } = [];

        public virtual bool Equals(Step? other)
        {
            if (other is null) return false;

            bool requisitesEqual = (Requisites == null && other.Requisites == null) ||
                                 (Requisites != null && other.Requisites != null &&
                                  Requisites.SequenceEqual(other.Requisites));

            return Number == other.Number &&
                   IsDone == other.IsDone &&
                   requisitesEqual;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Number);
            hash.Add(IsDone);
            if (Requisites != null)
            {
                foreach (var r in Requisites) hash.Add(r);
            }
            return hash.ToHashCode();
        }
    }
}
