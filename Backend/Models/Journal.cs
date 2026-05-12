using Backend.Models.Quests;

namespace Backend.Models
{
    public record class Journal
    {
        public MainQuest MainQuest { get; set; } = new MainQuest();
        public List<SideQuest> SideQuests { get; set; } = [];

        public virtual bool Equals(Journal? other)
        {
            if (other is null) return false;

            bool mainQuestEqual = (MainQuest == null && other.MainQuest == null) ||
                                  (MainQuest != null && MainQuest.Equals(other.MainQuest));

            bool sideQuestsEqual = (SideQuests == null && other.SideQuests == null) ||
                                   (SideQuests != null && other.SideQuests != null &&
                                    SideQuests.SequenceEqual(other.SideQuests));

            return mainQuestEqual && sideQuestsEqual;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(MainQuest);
            if (SideQuests != null)
            {
                foreach (var sq in SideQuests)
                {
                    hash.Add(sq);
                }
            }
            return hash.ToHashCode();
        }
    }
}
