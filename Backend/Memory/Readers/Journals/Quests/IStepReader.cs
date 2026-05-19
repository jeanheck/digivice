using Backend.Memory.Addresses.Journals.Quests;
using Backend.Memory.Resources.Journal.Quest;

namespace Backend.Memory.Readers.Journals.Quests
{
    public interface IStepReader
    {
        StepResource Read(StepAddresses addresses);
    }
}
