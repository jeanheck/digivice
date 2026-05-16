using Backend.Memory.Addresses.Quests;
using Backend.Memory.Resources.Quests;

namespace Backend.Memory.Readers
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
