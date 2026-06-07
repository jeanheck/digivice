using System.Text.Json;
using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Journals;
using Backend.Memory.Addresses.Parties;

namespace Backend.Memory.Repositories
{
    public class AddressesRepository(string dataDirectory) : IAddressesRepository
    {
        private readonly string dataDirectory = dataDirectory;
        private PlayerAddresses? playerAddresses;
        private PartyAddresses? partyAddresses;
        private DigimonStatusAddresses? digimonStatusAddresses;
        private DigimonsAddresses? digimonAddresses;
        private QuestAddresses? mainQuestAddresses;
        private QuestAddresses? sideQuestFolderBag;
        private QuestAddresses? sideQuestTreeBoots;
        private QuestAddresses? sideQuestFishingPole;
        private QuestAddresses? legendaryWeaponEternally;
        private QuestAddresses? legendaryWeaponInvincible;
        private QuestAddresses? legendaryWeaponMuramasa;
        private QuestAddresses? driAgentGuilmon;
        private QuestAddresses? driAgentAgumon;
        private AuctionAddresses? auctionAddresses;

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

        public List<QuestAddresses> GetAllLegendaryWeapons() =>
        [
            GetLegendaryWeaponEternally(),
            GetLegendaryWeaponInvincible(),
            GetLegendaryWeaponMuramasa()
        ];

        private QuestAddresses GetLegendaryWeaponEternally() =>
            LoadAndCache(ref legendaryWeaponEternally, "Quests/LegendaryWeapons/EternallyAddresses.json");

        private QuestAddresses GetLegendaryWeaponInvincible() =>
            LoadAndCache(ref legendaryWeaponInvincible, "Quests/LegendaryWeapons/InvincibleAddresses.json");

        private QuestAddresses GetLegendaryWeaponMuramasa() =>
            LoadAndCache(ref legendaryWeaponMuramasa, "Quests/LegendaryWeapons/MuramasaAddresses.json");

        public List<QuestAddresses> GetAllDriAgents() =>
        [
            GetDriAgentGuilmon(),
            GetDriAgentAgumon()
        ];

        private QuestAddresses GetDriAgentGuilmon() =>
            LoadAndCache(ref driAgentGuilmon, "Quests/DriAgents/GuilmonAddresses.json");

        private QuestAddresses GetDriAgentAgumon() =>
            LoadAndCache(ref driAgentAgumon, "Quests/DriAgents/AgumonAddresses.json");

        public AuctionAddresses GetAuctionAddresses() =>
            LoadAndCache(ref auctionAddresses, "AuctionAddresses.json");
    }
}
