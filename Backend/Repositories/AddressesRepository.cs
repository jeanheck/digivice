using System.Text.Json;
using Backend.Addresses;
using Backend.Addresses.Digimon;
using Backend.Interfaces;

namespace Backend.Repositories
{
    public class AddressesRepository : IAddressesRepository
    {
        private readonly string dataDirectory;
        private PlayerAddresses? playerAddresses;
        private PartyAddresses? partyAddresses;
        private DigimonAddresses? digimonAddresses;
        private Dictionary<int, DigimonBaseAddress>? digimonDefinitions;
        private QuestAddresses? mainQuestAddresses;
        private QuestAddresses? sideQuestFolderBag;
        private QuestAddresses? sideQuestTreeBoots;
        private QuestAddresses? sideQuestFishingPole;

        public AddressesRepository()
        {
            dataDirectory = Path.Combine(AppContext.BaseDirectory, "Database");
            if (!Directory.Exists(dataDirectory))
            {
                dataDirectory = Path.Combine(Environment.CurrentDirectory, "Database");
            }
        }

        private T LoadAndCache<T>(ref T? cacheField, string fileName) where T : class, new()
        {
            if (cacheField != null)
            {
                return cacheField;
            }

            var path = Path.Combine(dataDirectory, fileName);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Database file not found: {path}");
            }

            var json = File.ReadAllText(path);
            cacheField = JsonSerializer.Deserialize<T>(json) ?? new T();
            return cacheField;
        }

        public PlayerAddresses GetPlayerAddresses() =>
            LoadAndCache(ref playerAddresses, "PlayerAddresses.json");

        public PartyAddresses GetPartyAddresses() =>
            LoadAndCache(ref partyAddresses, "PartyAddresses.json");

        public DigimonAddresses GetDigimonAddresses() =>
            LoadAndCache(ref digimonAddresses, "DigimonAddresses.json");

        public Dictionary<int, DigimonBaseAddress> GetDigimonDefinitions() =>
            LoadAndCache(ref digimonDefinitions, "DigimonsAddresses.json");

        public QuestAddresses GetMainQuest() =>
            LoadAndCache(ref mainQuestAddresses, "Quests/MainQuestAddresses.json");

        public List<QuestAddresses> GetAllSideQuests() =>
        [
            GetSideQuestFolderBag(),
            GetSideQuestTreeBoots(),
            GetSideQuestFishingPole()
        ];

        private QuestAddresses GetSideQuestFolderBag() =>
            LoadAndCache(ref sideQuestFolderBag, "Quests/SideQuest/FolderBagAddresses.json");

        private QuestAddresses GetSideQuestTreeBoots() =>
            LoadAndCache(ref sideQuestTreeBoots, "Quests/SideQuest/TreeBootsAddresses.json");

        private QuestAddresses GetSideQuestFishingPole() =>
            LoadAndCache(ref sideQuestFishingPole, "Quests/SideQuest/FishingPoleAddresses.json");
    }
}
