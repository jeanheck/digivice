using Backend.Models.Quests;

namespace Backend.DetailedQuests.SideQuests
{
    public class FishingPole : SideQuest
    {
        private FishingPole() { }

        public static FishingPole Get()
        {
            return new FishingPole
            {
                Id = 3,
                Title = "The Fishing Pole",
                Description = "Obtain the Fishing Pole to start fishing for items and Digimon.",
                Prerequisites = new List<Requisite>
                {
                    new Requisite { Description = "Own the Folder Bag", IsDone = false }
                },
                Steps = new List<QuestStep>
                {
                    new QuestStep
                    {
                        Number = 1,
                        Description = "Talk to Tai Kong Wong at the Shell Beach",
                        IsCompleted = false
                    },
                    new QuestStep
                    {
                        Number = 2,
                        Description = "Help the Divermon at Divermon's Lake",
                        IsCompleted = false
                    },
                    new QuestStep
                    {
                        Number = 3,
                        Description = "Bring the items to Tai Kong Wong at the Shell Beach",
                        IsCompleted = false,
                        Prerequisites = new List<Requisite>
                        {
                            new Requisite { Description = "Bamboo Spear", IsDone = false, ItemKey = "BambooSpear", ItemType = "consumable" },
                            new Requisite { Description = "Spider Web", IsDone = false, ItemKey = "SpiderWeb", ItemType = "consumable" },
                            new Requisite { Description = "Red Snapper", IsDone = false, ItemKey = "RedSnapper", ItemType = "important" }
                        }
                    }
                }
            };
        }
    }
}
