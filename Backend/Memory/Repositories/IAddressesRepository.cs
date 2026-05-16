using Backend.Memory.Addresses;
using Backend.Memory.Repositories;
using Backend.Memory.Addresses.Digimon;

namespace Backend.Memory.Repositories
{
    public interface IAddressesRepository
    {
        PlayerAddresses GetPlayerAddresses();
        PartyAddresses GetPartyAddresses();
        DigimonAddresses GetDigimonAddresses();
        Dictionary<int, DigimonBaseAddress> GetDigimonDefinitions();
        QuestAddresses GetMainQuest();
        List<QuestAddresses> GetAllSideQuests();
    }
}
