using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Digimon;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public interface IAddressesReader
    {
        DigimonResource ReadDigimonResource(int slotIndex, int baseAddress, DigievolutionsAddresses digievolutionsAddresses);
    }
}
