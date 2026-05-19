using Backend.Memory.Addresses.Journals.Quests;
using Backend.Memory.Resources.Journal.Quest;

namespace Backend.Memory.Readers.Journal.Quest
{
    public class StepReader(
        IMemoryReader memoryReader, 
        IRequisiteReader requisiteReader) : IStepReader
    {
        public StepResource Read(StepAddresses addresses)
        {
            return new StepResource
            {
                Number = addresses.Number,
                Value = memoryReader.ReadByteSafe(addresses.Address, addresses.BitMask),
                Requisites = [.. addresses.Requisites.Select(requisiteReader.Read)]
            };
        }
    }
}
