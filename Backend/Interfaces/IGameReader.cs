using Backend.Addresses;
using Backend.Addresses.Digimon;
using Backend.Models.Quests;
using Backend.Resources;

namespace Backend.Interfaces
{
    public interface IGameReader
    {
        PlayerResource ReadPlayer(PlayerAddresses addresses);
        PartyResource ReadParty(PartyAddresses addresses);
        ImportantItemsResource ReadImportantItems(ImportantItemsAddresses addresses);
        ConsumableItemsResource ReadConsumableItems(ConsumableItemsAddresses addresses);
        DigimonResource ReadDigimon(int slotIndex, int baseAddress, DigievolutionsAddresses digievolutionsAddresses);
        Dictionary<int, byte> ReadQuestSteps(List<QuestStep> steps);
        void ReadQuestRequisites(IEnumerable<Requisite> requisites);
    }
}
