using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Journals;
using Backend.Memory.Addresses.Parties;

namespace Backend.Memory.Repositories
{
    public interface IAddressesRepository
    {
        PlayerAddresses GetPlayerAddresses();
        ImportantItemsAddresses GetImportantItemsAddresses();
        PartyAddresses GetPartyAddresses();
        DigimonStatusAddresses GetDigimonStatusAddresses();
        InBattleAddresses GetInBattleAddresses();
        EnemyAddresses GetEnemyAddresses();
        Dictionary<int, DigimonAddress> GetDigimonsAddresses();
        DigimonAddress? GetDigimonAddressById(int id);
        QuestAddresses GetMainQuest();
        List<QuestAddresses> GetAllSideQuests();
        List<QuestAddresses> GetAllLegendaryWeapons();
        List<QuestAddresses> GetAllDriAgents();
        Dictionary<string, AuctionAddresses> GetAuctionAddresses();
        Dictionary<string, NpcAddresses> GetNpcAddresses();
        CardBattleAddresses GetCardBattleAddresses();
    }
}
