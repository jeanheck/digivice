using Backend.Domain.Models;
using Backend.Memory.Repositories;
using Backend.Memory.Readers;
using Backend.Domain.Assemblers;

namespace Backend.Application.Services
{
    public class JournalStateService(
        IAddressesRepository addressesRepository,
        IQuestReader questReader)
    {
        public Journal GetJournal()
        {
            var mainQuestAddresses = addressesRepository.GetMainQuest();
            var allSideQuestsAddresses = addressesRepository.GetAllSideQuests();

            var mainQuestResource = questReader.Read(mainQuestAddresses);
            var sideQuestsResources = allSideQuestsAddresses
                .Select(questReader.Read);

            return JournalAssembler.Assemble(mainQuestResource, [.. sideQuestsResources]);
        }
    }
}
