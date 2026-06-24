using Backend.Domain.Models.Journals;

namespace Backend.Domain.Models
{
    public record class Journal
    {
        public Quest MainQuest { get; set; } = new();
        public List<Quest> SideQuests { get; set; } = [];
        public List<Quest> LegendaryWeapons { get; set; } = [];
        public List<Quest> DriAgents { get; set; } = [];
        public List<Auction> Auctions { get; set; } = [];

        public virtual bool Equals(Journal? other)
        {
            if (other is null) return false;

            bool mainQuestEqual = (MainQuest == null && other.MainQuest == null) ||
                                  (MainQuest != null && MainQuest.Equals(other.MainQuest));

            bool sideQuestsEqual = (SideQuests == null && other.SideQuests == null) ||
                                   (SideQuests != null && other.SideQuests != null &&
                                    SideQuests.SequenceEqual(other.SideQuests));

            bool legendaryWeaponsEqual = (LegendaryWeapons == null && other.LegendaryWeapons == null) ||
                                         (LegendaryWeapons != null && other.LegendaryWeapons != null &&
                                          LegendaryWeapons.SequenceEqual(other.LegendaryWeapons));

            bool driAgentsEqual = (DriAgents == null && other.DriAgents == null) ||
                                  (DriAgents != null && other.DriAgents != null &&
                                   DriAgents.SequenceEqual(other.DriAgents));

            bool auctionsEqual = (Auctions == null && other.Auctions == null) ||
                                 (Auctions != null && other.Auctions != null &&
                                  Auctions.SequenceEqual(other.Auctions));

            return mainQuestEqual && sideQuestsEqual && legendaryWeaponsEqual && driAgentsEqual && auctionsEqual;
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
            if (LegendaryWeapons != null)
            {
                foreach (var legendaryWeapon in LegendaryWeapons)
                {
                    hash.Add(legendaryWeapon);
                }
            }
            if (DriAgents != null)
            {
                foreach (var driAgent in DriAgents)
                {
                    hash.Add(driAgent);
                }
            }
            if (Auctions != null)
            {
                foreach (var auction in Auctions)
                {
                    hash.Add(auction);
                }
            }
            return hash.ToHashCode();
        }
    }
}
