using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Journals;
using Backend.Memory.Addresses.Parties;

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
        List<QuestAddresses> GetAllLegendaryWeapons();
        List<QuestAddresses> GetAllDriAgents();
    }
}
