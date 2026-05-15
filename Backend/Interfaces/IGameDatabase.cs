using Backend.Addresses;
using Backend.Addresses.Digimon;
using Backend.Models.Quests;

namespace Backend.Interfaces
{
    public interface IGameDatabase
    {
        PlayerAddresses GetPlayerAddresses();
        PartyAddresses GetPartyAddresses();
        DigimonAddresses GetDigimonAddresses();
        Dictionary<int, DigimonBaseAddress> GetDigimonDefinitions();
        MainQuest GetMainQuest();
        IEnumerable<SideQuest> GetAllSideQuests();
    }
}
