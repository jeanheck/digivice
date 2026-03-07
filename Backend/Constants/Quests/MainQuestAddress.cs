using System.Collections.Generic;
using Backend.Models.Quests;

namespace Backend.Constants.Quests
{
    public static class MainQuestAddress
    {
        // Quest progress discovered via memory card save comparison.
        // Step 1 uses bit 1 of 0x04B3B0 (shared byte with Tree Boots, which uses bits 2-7).
        // Step 2 uses bit 4 of 0x04B3B1.
        // Step 3 (final) doesn't change bit field — only activates the item flag.
        //
        // Because step 3 has no dedicated quest trigger, we use the item flag itself
        // (Fishing Pole in ImportantItems at 0x048DB5) to track completion.

        public static readonly List<QuestAddressStep> Steps = new()
        {
            new QuestAddressStep { Id = 1, Address = "0x0004B3B6", BitMask = "0x80" }, // Talk to Repeating Tom
            new QuestAddressStep { Id = 2, Address = "0x0004B3B7", BitMask = "0x01" }, // Talk to Seiryu Leader
            new QuestAddressStep { Id = 3, Address = "0x0004B3B7", BitMask = "0x02" }, // Defeat MasterTyrannomon
            new QuestAddressStep { Id = 4, Address = "0x0004B3E0", BitMask = "0x04" }, // Challenge the Seiryu Leader
            new QuestAddressStep { Id = 5, Address = "0x0004B63B", BitMask = "0x80" }, // Go to the East Station
            new QuestAddressStep { Id = 6, Address = "0x0004B3E0", BitMask = "0x40" }, // Defeat Mistery Player at Asuka Bridge
            new QuestAddressStep { Id = 7, Address = "0x0004B3BD", BitMask = "0x10" }, // Talk with Guilmon on Asuka Inn
            new QuestAddressStep { Id = 8, Address = "0x0004B3BD", BitMask = "0x20" }, // Talk with Guilmon on Forest Inn
            new QuestAddressStep { Id = 9, Address = "0x00048F3F", BitMask = "0x01" }, // Talk with Guilmon on Zephir Tower (Obtains Fake Blue Card)
            new QuestAddressStep { Id = 10, Address = "0x0004B3BD", BitMask = "0x40" }, // Return to the East Station with Fake Card
            new QuestAddressStep { Id = 11, Address = "0x0004B3BD", BitMask = "0x80" }, // Confront the Fake Card Guilmon
            new QuestAddressStep { Id = 12, Address = "0x00048F3E", BitMask = "0x01" }, // Talk to Guilmon at Forest Inn Basement (Obtains Real Blue Card)
            new QuestAddressStep { Id = 13, Address = "0x0004B3C5", BitMask = "0x20" }, // Return to the East Station with Real Card
        };
    }
}
