using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Digimon;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public interface IAddressesReader
    {
        PlayerResource ReadPlayerResource(PlayerAddresses addresses);
        PartyResource ReadPartyResource(PartyAddresses addresses);
        DigimonResource ReadDigimonResource(int slotIndex, int baseAddress, DigievolutionsAddresses digievolutionsAddresses);
        QuestResource ReadQuestResource(QuestAddresses addresses);
    }
}
