using System.Collections.Generic;
using Backend.Constants.Quests;

namespace Backend.Constants.Quests.SideQuests
{
    public static class FishingPoleAddress
    {
        public static readonly List<QuestAddressStep> Steps = new()
        {
            // MOCK addresses — need memory investigation to find real triggers
            new QuestAddressStep { Id = 1, Address = 0x00000000 }, // Step 1: Talk to NPC (MOCK)
            new QuestAddressStep { Id = 2, Address = 0x00048F42 }  // Step 2: Having the Fishing Pole (MOCK — using FolderBag address)
        };
    }
}
