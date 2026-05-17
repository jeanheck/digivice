using Backend.Memory.Repositories;
using Backend.Memory.Readers;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class QuestLoader(
        IAddressesRepository addressesRepository,
        IQuestReader questReader)
    {
        public QuestResource LoadMainQuest()
        {
            return questReader.Read(addressesRepository.GetMainQuest());
        }

        public List<QuestResource> LoadSideQuests()
        {
            return [.. addressesRepository.GetAllSideQuests().Select(questReader.Read)];
        }
    }
}
