using Backend.Memory.Addresses.Party.Digimon;
using Backend.Memory.Resources.Party.Digimon;

namespace Backend.Memory.Readers.Party.Digimon
{
    public interface IDigievolutionReader
    {
        DigievolutionResource Read(
            MemoryBlockReader memoryBlockReader,
            int digievolutionId,
            DigievolutionsAddresses digievolutionsAddresses);
    }
}
