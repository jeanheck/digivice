using Backend.Memory.Repositories;
using Backend.Memory.Readers.Journal;
using Backend.Memory.Resources.Journal;

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
