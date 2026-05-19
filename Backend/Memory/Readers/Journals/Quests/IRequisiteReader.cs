using Backend.Memory.Addresses.Journals.Quests;
using Backend.Memory.Resources.Journal.Quest;

namespace Backend.Memory.Readers.Journals.Quests
{
    public interface IRequisiteReader
    {
        RequisiteResource Read(RequisiteAddresses addresses);
    }
}
