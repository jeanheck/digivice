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
                        Description = "Talk to the Old Man at the Beach",
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
                        Description = "Bring the items back to the Old Man at the Beach",
                        IsCompleted = false,
                        Prerequisites = new List<Requisite>
                        {
                            new Requisite { Description = "Bamboo Spear", IsDone = false },
                            new Requisite { Description = "Spider Web", IsDone = false },
                            new Requisite { Description = "Divermon's reward item", IsDone = false }
                        }
                    }
                }
            };
        }
    }
}
