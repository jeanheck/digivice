using Backend.Models.Addresses;
using Backend.Services;

namespace Tests.Backend.Services
{
    public class DigievolutionStateServiceTests
    {
        [Fact]
        public void GetEquippedDigievolutions_ShouldCorrectlyMapLevels()
        {
            // We simulate a LogicBlock.
            // Addresses:
            // EquipedSlot1 = "2" (value: 0x10) -> length 2 -> offset 2 contains 0x10, 0x00
            // EquipedSlot2 = "4" (value: 0x20) -> offset 4 contains 0x20, 0x00
            // EquipedSlot3 = "6" (value: -1 meaning empty) -> offset 6 contains 0xFF, 0xFF
            // MaxUnlockedDigievolutions = "0" (fixed fallback 60? actually the fallback is hardcoded just if property null, so let's mock the value from memory).
            // Wait, MaxUnlocked is read safely from block. Wait! MaxUnlocked in the code:
            // `MemoryUtils.ReadInt32OffsetSafely(addresses?.MaxUnlockedDigievolutions, 60);`
            // UnlockedDigievolutionEntryStride = "8" -> safe fallback 8
            // UnlockedDigievolutionsStart = "0x50" -> safe fallback 0x50

            // Let's create a 100-byte logic block.
            var block = new byte[100];

            // Equipped Slot 1: ID 0x10 (16)
            block[2] = 0x10; block[3] = 0x00;
            // Equipped Slot 2: ID 0x20 (32)
            block[4] = 0x20; block[5] = 0x00;
            // Equipped Slot 3: Empty
            block[6] = 0xFF; block[7] = 0xFF;

            // Let's populate the dynamic table at 0x50 (80 in decimal).
            // Entry 0 (stride 8): ID = 0x10, Level = 15
            block[80] = 0x10; block[81] = 0x00; // ID
            block[82] = 15; block[83] = 0x00; // Level

            // Entry 1 (stride 8 -> 88): ID = 0x25 (Not equipped)
            block[88] = 0x25; block[89] = 0x00;
            block[90] = 30; block[91] = 0x00;

            // Entry 2 (stride 8 -> 96): ID = 0x20, Level = 99
            // Wait, block length is 100, so entryOffset + 2 is 98. 
            // We need a bigger block to fit entry 2 fully (96, 97, 98, 99).
            block = new byte[150];
            Array.Copy(new byte[] { 0x10, 0x00, 0x20, 0x00, 0xFF, 0xFF }, 0, block, 2, 6);
            block[80] = 0x10; block[82] = 15;
            block[88] = 0x25; block[90] = 30;
            block[96] = 0x20; block[98] = 99;

            var addresses = new DigievolutionsOffsets
            {
                EquipedSlot1 = "2",
                EquipedSlot2 = "4",
                EquipedSlot3 = "6",
                UnlockedDigievolutionsStart = "0x50", // 80 decimal
                UnlockedDigievolutionEntryStride = "8",
                MaxUnlockedDigievolutions = "3" // Only check 3 entries
            };

            var service = new DigievolutionStateService();
            var result = service.GetEquippedDigievolutions(block, addresses);

            Assert.Equal(3, result.Length);

            // Slot 1: ID 16, Level 15
            Assert.NotNull(result[0]);
            Assert.Equal(16, result[0]!.Id);
            Assert.Equal(15, result[0]!.Level);

            // Slot 2: ID 32, Level 99
            Assert.NotNull(result[1]);
            Assert.Equal(32, result[1]!.Id);
            Assert.Equal(99, result[1]!.Level);

            // Slot 3: Empty
            Assert.Null(result[2]);
        }
    }
}
