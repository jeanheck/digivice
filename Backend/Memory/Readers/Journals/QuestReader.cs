using Backend.Memory.Addresses.Journals;
using Backend.Memory.Readers.Journals.Quests;
using Backend.Memory.Resources.Journal;

namespace Backend.Memory.Readers.Journals
{
    public class QuestReader(
        IRequisiteReader requisiteReader,
        IStepReader stepReader) : IQuestReader
    {
        public QuestResource Read(QuestAddresses addresses)
        {
            return new QuestResource
            {
                Id = addresses.Id,
                Requisites = [.. addresses.Requisites.Select(requisiteReader.Read)],
                Steps = [.. addresses.Steps.Select(stepReader.Read)]
            };
        }
    }
}
