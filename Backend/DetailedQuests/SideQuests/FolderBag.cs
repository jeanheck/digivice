using System.Collections.Generic;
using Backend.Models.Quests;

namespace Backend.DetailedQuests.SideQuests
{
    public class FolderBag : SideQuest
    {
        public FolderBag()
        {
            Id = "side_folder_bag";
            Title = "The Folder Bag";
            Description = "Obtain the bag to start storing and collecting Digimon Cards.";

            Prerequisites = new List<Requisite>
            {
                new Requisite { Description = "Speak with Divermon in Asuka City", IsDone = false }
            };

            Steps = new List<QuestStep>
            {
                new QuestStep { Number = 1, Description = "Find the Folder Bag misplaced around the city.", IsCompleted = false }
            };
        }
    }
}
