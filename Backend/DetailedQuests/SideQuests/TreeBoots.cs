using Backend.Models.Quests;

namespace Backend.DetailedQuests.SideQuests
{
    public class TreeBoots : SideQuest
    {
        private TreeBoots() { }

        public static TreeBoots Get()
        {
            return new TreeBoots
            {
                Id = 2,
                Title = "The Tree Boots",
                Description = "Obtain the Tree Boots to unlock new areas by kicking trees.",
                Prerequisites = new List<Requisite>
                {
                    new Requisite { Description = "Own the Folder Bag", IsDone = false }
                },
                Steps = new List<QuestStep>
                {
                    new QuestStep
                    {
                        Number = 1,
                        Description = "Talk to the Soccer Kid Hide in Plug Cape",
                        IsCompleted = false
                    },
                    new QuestStep
                    {
                        Number = 2,
                        Description = "Find the lost Gabumon's card",
                        IsCompleted = false
                    },
                    new QuestStep
                    {
                        Number = 3,
                        Description = "Give the Gabumon's card to the Soccer Kid Hide",
                        IsCompleted = false
                    },
                    new QuestStep
                    {
                        Number = 4,
                        Description = "Talk to Waitress Debbie in Lamb Chop in Asuka City",
                        IsCompleted = false
                    },
                    new QuestStep
                    {
                        Number = 5,
                        Description = "Find Veemon on Wind Prairie",
                        IsCompleted = false
                    },
                    new QuestStep
                    {
                        Number = 6,
                        Description = "Play Hide and Seek and find Veemon on Kicking Forest",
                        IsCompleted = false
                    },
                    new QuestStep
                    {
                        Number = 7,
                        Description = "Receive the Tree Boots from Veemon",
                        IsCompleted = false
                    }
                }
            };
        }
    }
}
