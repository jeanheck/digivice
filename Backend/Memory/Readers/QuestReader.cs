using Backend.Memory.Addresses.Journals;
using Backend.Memory.Resources.Journals;
using Backend.Memory.Readers.Interfaces;

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
