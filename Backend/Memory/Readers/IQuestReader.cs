using Backend.Memory.Addresses.Journal;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public interface IQuestReader
    {
        QuestResource Read(QuestAddresses addresses);
    }
}
