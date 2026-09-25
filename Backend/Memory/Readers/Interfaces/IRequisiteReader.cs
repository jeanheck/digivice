using Backend.Memory.Addresses.Journals.Quests;
using Backend.Memory.Resources.Journals.Quests;

namespace Backend.Memory.Readers.Interfaces
{
    public interface IRequisiteReader
    {
        RequisiteResource Read(RequisiteAddresses addresses);
    }
}
