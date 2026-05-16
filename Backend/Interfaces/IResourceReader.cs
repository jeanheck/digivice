using Backend.Addresses;
using Backend.Addresses.Digimon;
using Backend.Resources;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IResourceReader
    {
        PlayerResource ReadPlayer(PlayerAddresses addresses);
        PartyResource ReadParty(PartyAddresses addresses);
        DigimonResource ReadDigimon(
            int slotIndex,
            int baseAddress,
            DigievolutionsAddresses digievolutionsAddresses);
        QuestResource ReadQuest(QuestAddresses addresses);
    }
}
