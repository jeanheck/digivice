using Backend.Domain.Models;
using Backend.Memory.Repositories;
using Backend.Memory.Readers;
using Backend.Domain.Models.Quests;
using Backend.Memory.Resources;

namespace Backend.Application.Services
{
    public class JournalStateService(
        IAddressesRepository addressesRepository,
        IResourceReader resourceReader)
    {
        public Journal GetJournal()
        {
            // 1. Get Definitions (Addresses)
            var mainQuestAddresses = addressesRepository.GetMainQuest();
            var allSideQuestsAddresses = addressesRepository.GetAllSideQuests();

            // 2. Read State (Resources)
            var mainQuestResource = resourceReader.ReadQuest(mainQuestAddresses);
            var sideQuestsResources = allSideQuestsAddresses
                .Select(resourceReader.ReadQuest)
                .ToList();

            // 3. Assemble Domain Models from Resources
            var mainQuest = ConvertResourceToModel(mainQuestResource);
            var sideQuests = sideQuestsResources.Select(ConvertResourceToModel).ToList();

            // 4. Normalize MainQuest Progression (Cascade)
            NormalizeMainQuestProgression(mainQuest);

            return new Journal
            {
                MainQuest = mainQuest,
                SideQuests = sideQuests
            };
        }

        private void NormalizeMainQuestProgression(Quest mainQuest)
        {
            // Completion cascade: If the next step is completed (> 0), 
            // the current step must also be considered completed.
            for (int i = mainQuest.Steps.Count - 2; i >= 0; i--)
            {
                if (mainQuest.Steps[i].Value == 0 && mainQuest.Steps[i + 1].Value > 0)
                {
                    mainQuest.Steps[i].Value = 1;
                }
            }
        }

        private static Quest ConvertResourceToModel(QuestResource resource)
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
