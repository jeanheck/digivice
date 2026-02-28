using Backend.Models.Quests;

namespace Backend.DetailedQuests.SideQuests
{
    public class KickingBoots : SideQuest
    {
        private KickingBoots() { }

        public static KickingBoots Get()
        {
            return new KickingBoots
            {
                Id = 2,
                Title = "The Kicking Boots",
                Description = "Obtain the Kicking Boots to unlock new areas by kicking obstacles.",
                Prerequisites = new List<Requisite>
                {
                    new Requisite { Description = "Own the Folder Bag", IsDone = false }
                },
                Steps = new List<QuestStep>
                {
                    new QuestStep
                    {
                        Number = 1,
                        Description = "Talk to the NPC who mentions the Kicking Boots",
                        IsCompleted = false
                    },
                    new QuestStep
                    {
                        Number = 2,
                        Description = "Obtain the Kicking Boots",
                        IsCompleted = false
                    }
                }
            };
        }
    }
}
