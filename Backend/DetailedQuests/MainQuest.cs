using Backend.Models.Quests;

namespace Backend.DetailedQuests
{
    public static class MainQuestFactory
    {
        private static List<QuestStep> Steps = new()
        {
            new QuestStep
            {
                Number = 1,
                Description = "Talk to Repeating Tom at Seiryu Tower, on Seiryu City",
                IsCompleted = false
            },
            new QuestStep
            {
                Number = 2,
                Description = "Talk to Seiryu Leader on Protocol Ruins",
                IsCompleted = false
            },
            new QuestStep
            {
                Number = 3,
                Description = "Defeat MasterTyrannomon on Tyranno Valley",
                IsCompleted = false
            },
            new QuestStep
            {
                Number = 4,
                Description = "Challenge the Seiryu Leader on Seiryu City",
                IsCompleted = false
            },
            new QuestStep
            {
                Number = 5,
                Description = "Go to the East Station to reach the next sector",
                IsCompleted = false
            },
            new QuestStep
            {
                Number = 6,
                Description = "Defeat the Mistery Player stopping you on the Asuka Bridge",
                IsCompleted = false
            },
            new QuestStep
            {
                Number = 7,
                Description = "Talk with the Guilmon on Asuka Inn 2F, on Asuka City",
                IsCompleted = false
            },
            new QuestStep
            {
                Number = 8,
                Description = "Talk with the Guilmon on Forest Inn",
                IsCompleted = false
            },
            new QuestStep
            {
                Number = 9,
                Description = "Talk with the Guilmon on Zephir Tower, on Seiriu City",
                IsCompleted = false
            },
            new QuestStep
            {
                Number = 10,
                Description = "Return to the East Station with the card",
                IsCompleted = false
            },
            new QuestStep
            {
                Number = 11,
                Description = "Confront the Guilmon on Seiryu City about the fake card",
                IsCompleted = false
            },
            new QuestStep
            {
                Number = 12,
                Description = "Talk to the Guilmon at the Forest Inn Basements",
                IsCompleted = false
            },
            new QuestStep
            {
                Number = 13,
                Description = "Return to the East Station with the real Blue Card",
                IsCompleted = false
            },
        };

        public static Backend.Models.Quests.MainQuest Get()
        {
            return new Backend.Models.Quests.MainQuest
            {
                Id = 1,
                Title = "The greatest Tamer!",
                Description = "Follow your path to become the greatest tamer of the Digital World!",
                Steps = Steps
            };
        }
    }
}
