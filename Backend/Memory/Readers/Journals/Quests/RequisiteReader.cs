using Backend.Memory.Addresses.Journals.Quests;
using Backend.Memory.Readers.Helpers;
using Backend.Memory.Resources.Journals.Quests;

namespace Backend.Memory.Readers.Journals.Quests
{
    public class RequisiteReader(IMemoryReader memoryReader) : IRequisiteReader
    {
        public RequisiteResource Read(RequisiteAddresses addresses)
        {
            return new RequisiteResource
            {
                Id = addresses.Id,
                Value = FlagByteHelper.Read(memoryReader, addresses.Address)
            };
        }
    }
}
