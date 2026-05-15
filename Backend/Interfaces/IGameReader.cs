using Backend.Addresses;
using Backend.Addresses.Digimon;
using Backend.Resources;
using Backend.Resources.Quests;
using Backend.Models.Quests;

namespace Backend.Interfaces
{
    public interface IGameReader
    {
        PlayerResource ReadPlayer(PlayerAddresses addresses);
        PartyResource ReadParty(PartyAddresses addresses);
        DigimonResource ReadDigimon(int slotIndex, int baseAddress, DigievolutionsAddresses digievolutionsAddresses);
        QuestResource ReadQuest(QuestAddresses addresses);
        Dictionary<int, byte> ReadQuestSteps(Quest quest);
    }
}
