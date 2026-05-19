using Backend.Memory.Addresses.Journals.Quests;
using Backend.Memory.Resources.Journals.Quests;

namespace Backend.Memory.Readers.Journals.Quests
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
