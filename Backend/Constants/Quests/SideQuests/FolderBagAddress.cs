using Backend.Models.Quests;

namespace Backend.Constants.Quests.SideQuests
{
    public static class FolderBagAddress
    {
        public const string QuestId = "side_folder_bag";
        public const string Title = "The Folder Bag";
        public const string Description = "Obtain the bag to start storing and collecting Digimon Cards.";

        // Real memory addresses to be discovered! Using mock offsets based on the 0x00048F42 area
        public static readonly Dictionary<int, int> StepCompletionAddresses = new()
        {
            { 1, 0x00048F42 }, // Step 1: Actually having the Folder Bag (we know this one flips to 1)
            { 2, 0x00048F43 }, // Mock: Return to Divermon
            { 3, 0x00048F44 }  // Mock: Receive Folder Bag officially
        };

        public static List<string> GetRequirements()
        {
            return new List<string> { "Speak with Divermon in Asuka City" };
        }

        public static List<QuestStep> GetDefaultSteps()
        {
            return new List<QuestStep>
            {
                new QuestStep { Number = 1, Description = "Find the Folder Bag misplaced around the city.", IsCompleted = false },
                new QuestStep { Number = 2, Description = "Return the bag to Divermon.", IsCompleted = false },
                new QuestStep { Number = 3, Description = "Divermon lets you keep the Folder Bag.", IsCompleted = false }
            };
        }
    }
}
