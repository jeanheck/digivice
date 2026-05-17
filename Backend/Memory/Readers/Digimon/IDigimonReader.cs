using Backend.Memory.Addresses.Digimon;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers.Digimon
{
    public interface IDigimonReader
    {
        DigimonResource Read(
            int slotIndex,
            int baseAddress,
            DigievolutionsAddresses digievolutionsAddresses);
    }
}
