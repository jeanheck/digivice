using Backend.Memory.Repositories;
using Backend.Memory.Readers.Journals;
using Backend.Memory.Resources.Journals;

namespace Backend.Application.Loaders.Journals
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
