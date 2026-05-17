using Backend.Memory.Addresses.Journal.Quest;
using Backend.Memory.Resources.Journal.Quest;

namespace Backend.Memory.Readers.Journal.Quest
{
    public interface IRequisiteReader
    {
        RequisiteResource Read(RequisiteAddresses addresses);
    }
}
