using Backend.Addresses;
using Backend.Addresses.Digimon;

namespace Backend.Interfaces
{
    public interface IGameDatabase
    {
        PlayerAddresses GetPlayerAddresses();
        PartyAddresses GetPartyAddresses();
        DigimonAddresses GetDigimonAddresses();
        Dictionary<int, DigimonBaseAddress> GetDigimonDefinitions();
        QuestAddresses GetMainQuest();
        List<QuestAddresses> GetAllSideQuests();
    }
}
