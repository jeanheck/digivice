using Backend.Memory.Addresses.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Parties.Digimons
{
    public interface IStoredDigievolutionReader
    {
        List<StoredDigievolutionResource> Read(
            MemoryBlockReader memoryBlockReader,
            DigievolutionsAddresses digievolutionsAddresses);
    }
}
