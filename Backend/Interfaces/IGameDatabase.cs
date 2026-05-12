using Backend.Models.Addresses;
using Backend.Models.Quests;

namespace Backend.Interfaces
{
    public interface IGameDatabase
    {
        PlayerAddresses GetPlayerAddresses();
        PartyAddresses GetPartyAddresses();
        ImportantItemsAddresses GetImportantItemsAddresses();
        ConsumableItemsAddresses GetConsumableItemsAddresses();
        DigimonAddresses GetDigimonAddresses();
        MainQuest GetMainQuest();
        SideQuest GetSideQuestFolderBag();
        SideQuest GetSideQuestTreeBoots();
        SideQuest GetSideQuestFishingPole();
    }
}
