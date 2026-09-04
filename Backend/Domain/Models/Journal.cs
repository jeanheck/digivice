using Backend.Domain.Models.Journals;

namespace Backend.Domain.Models
{
    public record class Journal
    {
        public Quest MainQuest { get; set; } = new();
        public List<Quest> SideQuests { get; set; } = [];
        public List<Quest> LegendaryWeapons { get; set; } = [];
        public List<Quest> DriAgents { get; set; } = [];
        public List<Quest> DuelIsland { get; set; } = [];
        public List<Npc> Npcs { get; set; } = [];

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

            bool duelIslandEqual = (DuelIsland == null && other.DuelIsland == null) ||
                                   (DuelIsland != null && other.DuelIsland != null &&
                                    DuelIsland.SequenceEqual(other.DuelIsland));

            bool npcsEqual = (Npcs == null && other.Npcs == null) ||
                             (Npcs != null && other.Npcs != null &&
                              Npcs.SequenceEqual(other.Npcs));

            return mainQuestEqual && sideQuestsEqual && legendaryWeaponsEqual && driAgentsEqual && duelIslandEqual && npcsEqual;
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
            if (DuelIsland != null)
            {
                foreach (var duelIslandQuest in DuelIsland)
                {
                    hash.Add(duelIslandQuest);
                }
            }
            if (Npcs != null)
            {
                foreach (var npc in Npcs)
                {
                    hash.Add(npc);
                }
            }
            return hash.ToHashCode();
        }
    }
}
