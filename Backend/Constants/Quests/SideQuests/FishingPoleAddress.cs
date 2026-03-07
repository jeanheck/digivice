using System.Collections.Generic;
using Backend.Models.Quests;

namespace Backend.Constants.Quests.SideQuests
{
    public static class FishingPoleAddress
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
            new QuestAddressStep { Id = 1, Address = "0x04B3B0", BitMask = "0x02" },  // bit 1: Talk to Old Man at Beach
            new QuestAddressStep { Id = 2, Address = "0x04B3B1", BitMask = "0x10" },  // bit 4: Help Divermon at Divermon's Lake
            new QuestAddressStep { Id = 3, Address = "0x048DB5", BitMask = null },   // legacy: byte == 1 → Fishing Pole obtained
        };
    }
}
