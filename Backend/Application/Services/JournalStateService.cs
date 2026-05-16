using Backend.Domain.Models;
using Backend.Memory.Repositories;
using Backend.Memory.Readers;
using Backend.Domain.Assemblers;

namespace Backend.Application.Services
{
    public class JournalStateService(
        IAddressesRepository addressesRepository,
        IAddressesReader addressesReader)
    {
        public Journal GetJournal()
        {
            var mainQuestAddresses = addressesRepository.GetMainQuest();
            var allSideQuestsAddresses = addressesRepository.GetAllSideQuests();

            var mainQuestResource = addressesReader.ReadQuestResource(mainQuestAddresses);
            var sideQuestsResources = allSideQuestsAddresses
                .Select(addressesReader.ReadQuestResource)
                .ToList();

            return JournalAssembler.Assemble(mainQuestResource, sideQuestsResources);
        }
    }
}
