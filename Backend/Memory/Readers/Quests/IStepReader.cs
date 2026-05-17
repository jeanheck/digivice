using Backend.Memory.Addresses.Journal.Quest;
using Backend.Memory.Resources.Quests;

namespace Backend.Memory.Readers.Quests
{
    public interface IStepReader
    {
        StepResource Read(StepAddresses addresses);
    }
}
