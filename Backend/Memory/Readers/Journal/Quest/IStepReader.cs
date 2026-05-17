using Backend.Memory.Addresses.Journal.Quest;
using Backend.Memory.Resources.Journal.Quest;

namespace Backend.Memory.Readers.Journal.Quest
{
    public interface IStepReader
    {
        StepResource Read(StepAddresses addresses);
    }
}
