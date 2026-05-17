using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Journal;
using Backend.Memory.Addresses.Party;

namespace Backend.Memory.Repositories
{
    public interface IAddressesRepository
    {
        PlayerAddresses GetPlayerAddresses();
        PartyAddresses GetPartyAddresses();
        DigimonStatusAddresses GetDigimonStatusAddresses();
        DigimonsAddresses GetDigimonsAddresses();
        DigimonAddress? GetDigimonAddressById(int id);
        QuestAddresses GetMainQuest();
        List<QuestAddresses> GetAllSideQuests();
    }
}
