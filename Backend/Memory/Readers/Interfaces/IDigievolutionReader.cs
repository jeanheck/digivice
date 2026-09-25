using Backend.Memory.Addresses.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Interfaces
{
    public interface IDigievolutionReader
    {
        DigievolutionResource Read(
            MemoryBlockReader memoryBlockReader,
            int digievolutionId,
            DigievolutionsAddresses digievolutionsAddresses);
    }
}
