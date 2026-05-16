using Backend.Memory.Addresses;
using Backend.Memory.Readers;
using Backend.Memory.Addresses.Digimon;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public interface IResourceReader
    {
        PlayerResource ReadPlayer(PlayerAddresses addresses);
        PartyResource ReadParty(PartyAddresses addresses);
        DigimonResource ReadDigimon(int slotIndex, int baseAddress, DigievolutionsAddresses digievolutionsAddresses);
        QuestResource ReadQuest(QuestAddresses addresses);
    }
}
