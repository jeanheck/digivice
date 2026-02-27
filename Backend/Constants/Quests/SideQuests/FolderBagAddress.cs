using System.Collections.Generic;
using Backend.Constants.Quests;

namespace Backend.Constants.Quests.SideQuests
{
    public static class FolderBagAddress
    {
        public static readonly List<QuestAddressStep> Steps = new()
        {
            new QuestAddressStep { Id = 1, Address = 0x00048F42 } // Step 1: Actually having the Folder Bag
        };
    }
}
