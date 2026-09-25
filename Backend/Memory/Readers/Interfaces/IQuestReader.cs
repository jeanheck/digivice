using Backend.Memory.Addresses.Journals;
using Backend.Memory.Resources.Journals;

namespace Backend.Memory.Readers.Interfaces
{
    public interface IQuestReader
    {
        QuestResource Read(QuestAddresses addresses);
    }
}
