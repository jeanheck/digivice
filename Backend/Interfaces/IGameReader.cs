using Backend.Models.Addresses;
using Backend.Models.Quests;
using Backend.Models.Resources;

namespace Backend.Interfaces
{
    public interface IGameReader
    {
        PlayerResource ReadPlayer(PlayerAddresses addresses);
        PartyResource ReadParty(PartyAddresses addresses);
        ImportantItemsResource ReadImportantItems(ImportantItemsAddresses addresses);
        ConsumableItemsResource ReadConsumableItems(ConsumableItemsAddresses addresses);
        DigimonResource ReadDigimonResource(int slotIndex, int baseAddress, DigimonAddresses addresses);
        Dictionary<int, byte> ReadQuestSteps(List<QuestStep> steps);
    }
}
