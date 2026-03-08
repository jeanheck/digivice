using Backend.Models.Quests;

namespace Tests.Backend.Models
{
    public class QuestStepTests
    {
        [Fact]
        public void Equals_ShouldReturnTrue_ForIdenticalProperties()
        {
            var step1 = new QuestStep { Number = 1, Description = "A", Address = "0x20", BitMask = "0x01", IsCompleted = false };
            var step2 = new QuestStep { Number = 1, Description = "A", Address = "0x20", BitMask = "0x01", IsCompleted = false };

            Assert.True(step1.Equals(step2));
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentCompletionState()
        {
            var step1 = new QuestStep { Number = 1, Description = "A", Address = "0x20", BitMask = "0x01", IsCompleted = false };
            var step2 = new QuestStep { Number = 1, Description = "A", Address = "0x20", BitMask = "0x01", IsCompleted = true };

            Assert.False(step1.Equals(step2));
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentMaskOrAddress()
        {
            var step1 = new QuestStep { Number = 1, Description = "A", Address = "0x20", BitMask = "0x01", IsCompleted = false };
            var step2 = new QuestStep { Number = 1, Description = "A", Address = "0x20", BitMask = "0x02", IsCompleted = false };
            var step3 = new QuestStep { Number = 1, Description = "A", Address = "0x30", BitMask = "0x01", IsCompleted = false };

            Assert.False(step1.Equals(step2));
            Assert.False(step1.Equals(step3));
        }
    }
}
