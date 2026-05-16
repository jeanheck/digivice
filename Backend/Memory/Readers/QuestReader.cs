using Backend.Memory.Addresses;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Quests;

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
