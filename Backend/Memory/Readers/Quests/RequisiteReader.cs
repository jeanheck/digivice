using Backend.Memory.Addresses.Journal.Quest;
using Backend.Memory.Resources.Quests;

namespace Backend.Memory.Readers.Quests
{
    public class RequisiteReader(IMemoryReader memoryReader) : IRequisiteReader
    {
        public RequisiteResource Read(RequisiteAddresses addresses)
        {
            return new RequisiteResource
            {
                Id = addresses.Id,
                Value = memoryReader.ReadByteSafe(addresses.Address)
            };
        }
    }
}
