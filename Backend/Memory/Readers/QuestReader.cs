using Backend.Memory.Addresses.Journal;
using Backend.Memory.Readers.Quests;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
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
