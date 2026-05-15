using Backend.Addresses;
using Backend.Addresses.Digimon;
using Backend.Resources;
using Backend.Models.Quests;

namespace Backend.Interfaces
{
    public interface IGameReader
    {
        PlayerResource ReadPlayer(PlayerAddresses addresses);
        PartyResource ReadParty(PartyAddresses addresses);
        DigimonResource ReadDigimon(int slotIndex, int baseAddress, DigievolutionsAddresses digievolutionsAddresses);
        Dictionary<int, byte> ReadQuestSteps(Quest quest);
    }
}
