using Backend.Memory.Addresses.Journals.Quests;
using Backend.Memory.Resources.Journals.Quests;

namespace Backend.Memory.Readers.Journals.Quests
{
    public interface IRequisiteReader
    {
        RequisiteResource Read(RequisiteAddresses addresses);
    }
}
