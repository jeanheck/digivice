using System.Text.Json;
using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Journals;
using Backend.Memory.Addresses.Parties;

namespace Backend.Memory.Repositories
{
    public class AddressesRepository(string dataDirectory) : IAddressesRepository
    {
        private PlayerAddresses? playerAddresses;
        private PartyAddresses? partyAddresses;
        private DigimonStatusAddresses? digimonStatusAddresses;
        private DigimonInCombatAddresses? digimonInCombatAddresses;
        private Dictionary<int, DigimonAddress>? digimonAddresses;
        private QuestAddresses? mainQuestAddresses;
        private List<QuestAddresses>? sideQuestAddresses;
        private List<QuestAddresses>? legendaryWeaponAddresses;
        private List<QuestAddresses>? driAgentAddresses;
        private Dictionary<string, AuctionAddresses>? auctionAddresses;

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

        private List<QuestAddresses> LoadAllQuestAddressesFromFolder(
            ref List<QuestAddresses>? cacheField,
            string relativeFolder)
        {
            if (cacheField != null)
            {
                return cacheField;
            }

            var folderPath = Path.Combine(dataDirectory, relativeFolder);
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"Quest addresses folder not found: {folderPath}");
            }

            List<QuestAddresses> loaded = [];
            foreach (var filePath in Directory.EnumerateFiles(folderPath, "*.json")
                .OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase))
            {
                var json = File.ReadAllText(filePath);
                loaded.Add(JsonSerializer.Deserialize<QuestAddresses>(json) ?? new QuestAddresses());
            }

            cacheField = loaded;
            return cacheField;
        }

        public PlayerAddresses GetPlayerAddresses() =>
            LoadAndCache(ref playerAddresses, "PlayerAddresses.json");

        public PartyAddresses GetPartyAddresses() =>
            LoadAndCache(ref partyAddresses, "PartyAddresses.json");

        public DigimonStatusAddresses GetDigimonStatusAddresses() =>
            LoadAndCache(ref digimonStatusAddresses, "Parties/DigimonStatusAddresses.json");

        public DigimonInCombatAddresses GetDigimonInCombatAddresses() =>
            LoadAndCache(ref digimonInCombatAddresses, "Parties/DigimonInCombatAddresses.json");

        public Dictionary<int, DigimonAddress> GetDigimonsAddresses() =>
            LoadAndCache(ref digimonAddresses, "Parties/DigimonsAddresses.json");

        public DigimonAddress? GetDigimonAddressById(int id) =>
            GetDigimonsAddresses().TryGetValue(id, out var digimonAddress)
                ? digimonAddress
                : null;

        public QuestAddresses GetMainQuest() =>
            LoadAndCache(ref mainQuestAddresses, "Quests/MainQuestAddresses.json");

        public List<QuestAddresses> GetAllSideQuests() =>
            LoadAllQuestAddressesFromFolder(ref sideQuestAddresses, "Quests/SideQuests");

        public List<QuestAddresses> GetAllLegendaryWeapons() =>
            LoadAllQuestAddressesFromFolder(ref legendaryWeaponAddresses, "Quests/LegendaryWeapons");

        public List<QuestAddresses> GetAllDriAgents() =>
            LoadAllQuestAddressesFromFolder(ref driAgentAddresses, "Quests/DriAgents");

        public Dictionary<string, AuctionAddresses> GetAuctionAddresses() =>
            LoadAndCache(ref auctionAddresses, "AuctionAddresses.json");
    }
}
