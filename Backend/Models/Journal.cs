using Backend.Models.Quests;

namespace Backend.Models
{
    public class Journal
    {
        public MainQuest MainQuest { get; set; } = new MainQuest();
        public List<SideQuest> SideQuests { get; set; } = new();

        public override bool Equals(object? obj)
        {
            if (obj is not Journal other) return false;

            bool mainQuestEqual = (MainQuest == null && other.MainQuest == null) ||
                                  (MainQuest != null && MainQuest.Equals(other.MainQuest));

            bool sideQuestsEqual = (SideQuests == null && other.SideQuests == null) ||
                                   (SideQuests != null && other.SideQuests != null &&
                                    SideQuests.Count == other.SideQuests.Count &&
                                    SideQuests.SequenceEqual(other.SideQuests));

            return mainQuestEqual && sideQuestsEqual;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(MainQuest);
            if (SideQuests != null)
            {
                foreach (var sq in SideQuests) hash.Add(sq);
            }
            return hash.ToHashCode();
        }
    }
}
