using Backend.Models.Quests;

namespace Backend.DetailedQuests.SideQuests
{
    public class FolderBag : SideQuest
    {
        private FolderBag() { }

        public static FolderBag Get()
        {
            return new FolderBag
            {
                Id = 1,
                Title = "The Folder Bag",
                Description = "Obtain the bag to start storing and collecting Digimon Cards.",
                Prerequisites = new List<Requisite> { },
                Steps = new List<QuestStep>
                {
                    new QuestStep
                    {
                        Number = 1,
                        Description = "Speak with Divermon in the Yellow Cruiser located in Asuka City",
                        IsCompleted = false
                    }
                }
            };
        }
    }
}
