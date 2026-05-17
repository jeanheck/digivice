using Backend.Memory.Addresses.Journal.Quest;
using Backend.Memory.Resources.Journal.Quest;

namespace Backend.Memory.Readers.Quests
{
    public interface IRequisiteReader
    {
        RequisiteResource Read(RequisiteAddresses addresses);
    }
}
