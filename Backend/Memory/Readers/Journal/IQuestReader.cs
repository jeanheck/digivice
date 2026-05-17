using Backend.Memory.Addresses.Journal;
using Backend.Memory.Resources.Journal;

namespace Backend.Memory.Readers.Journal
{
    public interface IQuestReader
    {
        QuestResource Read(QuestAddresses addresses);
    }
}
