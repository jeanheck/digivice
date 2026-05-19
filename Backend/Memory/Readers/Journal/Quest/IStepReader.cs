using Backend.Memory.Addresses.Journals.Quests;
using Backend.Memory.Resources.Journal.Quest;

namespace Backend.Memory.Readers.Journal.Quest
{
    public interface IStepReader
    {
        StepResource Read(StepAddresses addresses);
    }
}
