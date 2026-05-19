using Backend.Memory.Addresses.Parties.Digimons;
using Backend.Memory.Resources.Party.Digimon;

namespace Backend.Memory.Readers.Parties.Digimons
{
    public interface IDigievolutionReader
    {
        DigievolutionResource Read(
            MemoryBlockReader memoryBlockReader,
            int digievolutionId,
            DigievolutionsAddresses digievolutionsAddresses);
    }
}
