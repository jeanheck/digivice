using System.Collections.Generic;
using Backend.Constants.Quests;

namespace Backend.Constants.Quests.SideQuests
{
    public static class TreeBootsAddress
    {
        public static readonly List<QuestAddressStep> Steps = new()
        {
            new QuestAddressStep { Id = 1, Address = 0x00044C11 }, // Step 1: Talk to Soccer Kid Hide
            new QuestAddressStep { Id = 2, Address = 0x00044C51 }, // Step 2: Find Gabumon's card
            new QuestAddressStep { Id = 3, Address = 0x0004B3B6 }, // Step 3: Return card to Hide
            new QuestAddressStep { Id = 4, Address = 0x0004DE40 }, // Step 4: Talk to Waitress Debbie
            new QuestAddressStep { Id = 5, Address = 0x00044BDE }, // Step 5: Find Veemon on Wind Prairie
            new QuestAddressStep { Id = 6, Address = 0x00044C01 }, // Step 6: Hide&Seek Veemon on Kicking Forest
            new QuestAddressStep { Id = 7, Address = 0x0004B3B1 }, // Step 7: Receive Tree Boots from Veemon
        };
    }
}
