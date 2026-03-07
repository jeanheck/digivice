using System.Collections.Generic;
using Backend.Models.Quests;

namespace Backend.Constants.Quests.SideQuests
{
    public static class TreeBootsAddress
    {
        // Quest progress stored at RAM 0x04B3B0 as a bit field.
        // Discovered via memory card save comparison.
        //
        // Steps 1-3: cumulative bits (each bit stays on permanently)
        // Steps 4-6: rotating phase bits (only the latest stays on; previous clears)
        //   → We use OR masks so that advancing past a step still counts it as done.
        // Step 7: bit 0 of next byte (0x04B3B1)
        //
        // Byte layout at 0x04B3B0:
        //   bit 2 = Step 1 (Talk to Hide)        — cumulative
        //   bit 3 = Step 2 (Find card)            — cumulative
        //   bit 4 = Step 3 (Return card)          — cumulative
        //   bit 5 = Step 4 phase (Talk to Debbie) — rotating
        //   bit 6 = Step 5 phase (Find Veemon)    — rotating
        //   bit 7 = Step 6 phase (Hide & Seek)    — rotating

        public static readonly List<QuestAddressStep> Steps = new()
        {
            // Cumulative flags — single bit each
            new QuestAddressStep { Id = 1, Address = "0x04B3B0", BitMask = "0x04" },  // bit 2
            new QuestAddressStep { Id = 2, Address = "0x04B3B0", BitMask = "0x08" },  // bit 3
            new QuestAddressStep { Id = 3, Address = "0x04B3B0", BitMask = "0x10" },  // bit 4

            // Rotating phase flags — OR mask includes this bit and all subsequent phase bits
            new QuestAddressStep { Id = 4, Address = "0x04B3B0", BitMask = "0xE0" },  // bits 5|6|7 → done if any phase past Debbie
            new QuestAddressStep { Id = 5, Address = "0x04B3B0", BitMask = "0xC0" },  // bits 6|7   → done if any phase past Veemon
            new QuestAddressStep { Id = 6, Address = "0x04B3B0", BitMask = "0x80" },  // bit 7      → done if Hide&Seek completed

            // Next byte — completion flag
            new QuestAddressStep { Id = 7, Address = "0x04B3B1", BitMask = "0x01" },  // bit 0 → Received Tree Boots
        };
    }
}
