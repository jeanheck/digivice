using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Digimon;

namespace Backend.Memory.Repositories
{
    public interface IAddressesRepository
    {
        PlayerAddresses GetPlayerAddresses();
        PartyAddresses GetPartyAddresses();
        DigimonStatusAddresses GetDigimonStatusAddresses();
        DigimonAddresses GetDigimonAddresses();
        QuestAddresses GetMainQuest();
        List<QuestAddresses> GetAllSideQuests();
    }
}
