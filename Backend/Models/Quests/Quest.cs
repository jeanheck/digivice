namespace Backend.Models.Quests
{
    public abstract record class Quest
    {
        public string? Id { get; set; }
        public List<Requisite> Requisites { get; set; } = [];
        public List<QuestStep> Steps { get; set; } = [];

        public virtual bool Equals(Quest? other)
        {
            if (other is null) return false;

            bool requisitesEqual = (Requisites == null && other.Requisites == null) ||
                                 (Requisites != null && other.Requisites != null &&
                                  Requisites.SequenceEqual(other.Requisites));

            bool stepsEqual = (Steps == null && other.Steps == null) ||
                              (Steps != null && other.Steps != null &&
                               Steps.SequenceEqual(other.Steps));

            return Id == other.Id &&
                   requisitesEqual &&
                   stepsEqual;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Id);
            if (Requisites != null)
            {
                foreach (var r in Requisites) hash.Add(r);
            }
            if (Steps != null)
            {
                foreach (var s in Steps) hash.Add(s);
            }
            return hash.ToHashCode();
        }
    }
}
