using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Digimon;
using Backend.Memory.Addresses.Journal;

namespace Backend.Memory.Repositories
{
    public interface IAddressesRepository
    {
        PlayerAddresses GetPlayerAddresses();
        PartyAddresses GetPartyAddresses();
        DigimonStatusAddresses GetDigimonStatusAddresses();
        DigimonAddresses GetDigimonsAddresses();
        DigimonAddress? GetDigimonAddressById(int id);
        QuestAddresses GetMainQuest();
        List<QuestAddresses> GetAllSideQuests();
    }
}
