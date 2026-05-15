using System.Text.Json;
using Backend.Addresses;
using Backend.Addresses.Digimon;
using Backend.Models.Quests;
using Backend.Interfaces;

namespace Backend.Services
{
    public class GameDatabase : IGameDatabase
    {
        private readonly string dataDirectory;
        private PlayerAddresses? playerAddresses;
        private PartyAddresses? partyAddresses;
        private ImportantItemsAddresses? importantItemsAddresses;
        private ConsumableItemsAddresses? consumableItemsAddresses;
        private DigimonAddresses? digimonAddresses;
        private Dictionary<int, DigimonBaseAddress>? digimonDefinitions;
        private MainQuest? mainQuest;
        private SideQuest? sideQuestFolderBag;
        private SideQuest? sideQuestTreeBoots;
        private SideQuest? sideQuestFishingPole;

        public GameDatabase()
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
        public MainQuest GetMainQuest() =>
            LoadAndCache(ref mainQuest, "Quests/MainQuestAddresses.json");
        public IEnumerable<SideQuest> GetAllSideQuests()
        {
            yield return GetSideQuestFolderBag();
            yield return GetSideQuestTreeBoots();
            yield return GetSideQuestFishingPole();
        }
        public ImportantItemsAddresses GetImportantItemsAddresses() =>
            LoadAndCache(ref importantItemsAddresses, "ImportantItemsAddresses.json");
        public ConsumableItemsAddresses GetConsumableItemsAddresses() =>
            LoadAndCache(ref consumableItemsAddresses, "ConsumableItemsAddresses.json");
        private SideQuest GetSideQuestFolderBag() =>
            LoadAndCache(ref sideQuestFolderBag, "Quests/SideQuest/FolderBagAddresses.json");
        private SideQuest GetSideQuestTreeBoots() =>
            LoadAndCache(ref sideQuestTreeBoots, "Quests/SideQuest/TreeBootsAddresses.json");
        private SideQuest GetSideQuestFishingPole() =>
            LoadAndCache(ref sideQuestFishingPole, "Quests/SideQuest/FishingPoleAddresses.json");
    }
}
