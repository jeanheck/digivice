using System.Text.Json;
using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Journals;
using Backend.Memory.Addresses.Parties;

namespace Backend.Memory.Repositories
{
    public class AddressesRepository : IAddressesRepository
    {
        private readonly string dataDirectory;
        private PlayerAddresses? playerAddresses;
        private PartyAddresses? partyAddresses;
        private DigimonStatusAddresses? digimonStatusAddresses;
        private DigimonsAddresses? digimonAddresses;
        private QuestAddresses? mainQuestAddresses;
        private QuestAddresses? sideQuestFolderBag;
        private QuestAddresses? sideQuestTreeBoots;
        private QuestAddresses? sideQuestFishingPole;

        public AddressesRepository(string dataDirectory)
        {
            this.dataDirectory = dataDirectory;
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

        public DigimonStatusAddresses GetDigimonStatusAddresses() =>
            LoadAndCache(ref digimonStatusAddresses, "Parties/DigimonStatusAddresses.json");

        public DigimonsAddresses GetDigimonsAddresses() =>
            LoadAndCache(ref digimonAddresses, "Parties/DigimonsAddresses.json");

        public DigimonAddress? GetDigimonAddressById(int id) =>
            GetDigimonsAddresses().Digimons.FirstOrDefault(d => d.Id == id);

        public QuestAddresses GetMainQuest() =>
            LoadAndCache(ref mainQuestAddresses, "Quests/MainQuestAddresses.json");

        public List<QuestAddresses> GetAllSideQuests() =>
        [
            GetSideQuestFolderBag(),
            GetSideQuestTreeBoots(),
            GetSideQuestFishingPole()
        ];

        private QuestAddresses GetSideQuestFolderBag() =>
            LoadAndCache(ref sideQuestFolderBag, "Quests/SideQuests/FolderBagAddresses.json");

        private QuestAddresses GetSideQuestTreeBoots() =>
            LoadAndCache(ref sideQuestTreeBoots, "Quests/SideQuests/TreeBootsAddresses.json");

        private QuestAddresses GetSideQuestFishingPole() =>
            LoadAndCache(ref sideQuestFishingPole, "Quests/SideQuests/FishingPoleAddresses.json");
    }
}
