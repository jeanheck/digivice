using Backend.Models;
using Backend.Models.Quests;
using Backend.Interfaces;
using Backend.Resources;

namespace Backend.Services
{
    public class JournalStateService(
        IGameDatabase gameDatabase,
        IGameReader gameReader)
    {
        public Journal GetJournal()
        {
            // 1. Get Definitions (Addresses)
            var mainQuestAddresses = gameDatabase.GetMainQuest();
            var allSideQuestsAddresses = gameDatabase.GetAllSideQuests();

            // 2. Read State (Resources)
            var mainQuestResource = gameReader.ReadQuest(mainQuestAddresses);
            var sideQuestsResources = allSideQuestsAddresses
                .Select(gameReader.ReadQuest)
                .ToList();

            // 3. Assemble Domain Models from Resources
            return new Journal
            {
                MainQuest = AssembleQuest(mainQuestResource),
                SideQuests = sideQuestsResources.Select(AssembleQuest).ToList()
            };
        }

        private static Quest AssembleQuest(QuestResource resource)
        {
            return new Quest
            {
                Id = resource.Id,
                Requisites = resource.Requisites.Select(r => new Requisite
                {
                    Id = r.Id,
                    Value = r.Value
                }).ToList(),
                Steps = resource.Steps.Select(s => new Step
                {
                    Number = s.Number,
                    Value = s.Value,
                    Requisites = s.Requisites.Select(r => new Requisite
                    {
                        Id = r.Id,
                        Value = r.Value
                    }).ToList()
                }).ToList()
            };
        }
    }
}
