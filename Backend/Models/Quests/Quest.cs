namespace Backend.Models.Quests
{
    public abstract class Quest
    {
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Requisite> Prerequisites { get; set; } = new();
        public List<QuestStep> Steps { get; set; } = new();

        public override bool Equals(object? obj)
        {
            if (obj is not Quest other) return false;

            bool prereqsEqual = (Prerequisites == null && other.Prerequisites == null) ||
                                (Prerequisites != null && other.Prerequisites != null &&
                                 Prerequisites.Count == other.Prerequisites.Count &&
                                 Prerequisites.SequenceEqual(other.Prerequisites));

            bool stepsEqual = (Steps == null && other.Steps == null) ||
                              (Steps != null && other.Steps != null &&
                               Steps.Count == other.Steps.Count &&
                               Steps.SequenceEqual(other.Steps));

            return Id == other.Id &&
                   Title == other.Title &&
                   Description == other.Description &&
                   prereqsEqual &&
                   stepsEqual;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Id);
            hash.Add(Title);
            hash.Add(Description);
            if (Prerequisites != null)
            {
                foreach (var p in Prerequisites) hash.Add(p);
            }
            if (Steps != null)
            {
                foreach (var s in Steps) hash.Add(s);
            }
            return hash.ToHashCode();
        }
    }
}
