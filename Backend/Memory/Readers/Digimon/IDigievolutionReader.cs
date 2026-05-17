using Backend.Memory.Addresses.Digimon;
using Backend.Memory.Resources.Digimon;

namespace Backend.Memory.Readers.Digimon
{
    public interface IDigievolutionReader
    {
        DigievolutionResource Read(
            MemoryBlockReader memoryBlockReader,
            int digievolutionId,
            DigievolutionsAddresses digievolutionsAddresses);
    }
}
