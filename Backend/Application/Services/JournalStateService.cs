using Backend.Domain.Models;
using Backend.Memory.Repositories;
using Backend.Memory.Readers;
using Backend.Domain.Assemblers;

namespace Backend.Application.Services
{
    public class JournalStateService(
        IAddressesRepository addressesRepository,
        IResourceReader resourceReader)
    {
        public Journal GetJournal()
        {
            var mainQuestAddresses = addressesRepository.GetMainQuest();
            var allSideQuestsAddresses = addressesRepository.GetAllSideQuests();

            var mainQuestResource = resourceReader.ReadQuest(mainQuestAddresses);
            var sideQuestsResources = allSideQuestsAddresses
                .Select(resourceReader.ReadQuest)
                .ToList();

            return JournalAssembler.Assemble(mainQuestResource, sideQuestsResources);
        }
    }
}
