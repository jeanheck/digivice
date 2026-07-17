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
        private QuestAddresses? legendaryWeaponSuperNova;
        private QuestAddresses? legendaryWeaponPunishment;
        private QuestAddresses? driAgentGuilmon;
        private QuestAddresses? driAgentAgumon;
        private QuestAddresses? driAgentVeemon;
        private QuestAddresses? driAgentKumamon;
        private QuestAddresses? driAgentMonmon;
        private QuestAddresses? driAgentKotemon;
        private QuestAddresses? driAgentRenamon;
        private QuestAddresses? driAgentPatamon;
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

        private QuestAddresses GetDriAgentGuilmon() =>
            LoadAndCache(ref driAgentGuilmon, "Quests/DriAgents/DriAgentGuilmonAddresses.json");
        private QuestAddresses GetDriAgentAgumon() =>
            LoadAndCache(ref driAgentAgumon, "Quests/DriAgents/DriAgentAgumonAddresses.json");
        private QuestAddresses GetDriAgentVeemon() =>
            LoadAndCache(ref driAgentVeemon, "Quests/DriAgents/DriAgentVeemonAddresses.json");
        private QuestAddresses GetDriAgentKumamon() =>
            LoadAndCache(ref driAgentKumamon, "Quests/DriAgents/DriAgentKumamonAddresses.json");
        private QuestAddresses GetDriAgentMonmon() =>
            LoadAndCache(ref driAgentMonmon, "Quests/DriAgents/DriAgentMonmonAddresses.json");
        private QuestAddresses GetDriAgentKotemon() =>
            LoadAndCache(ref driAgentKotemon, "Quests/DriAgents/DriAgentKotemonAddresses.json");
        private QuestAddresses GetDriAgentRenamon() =>
            LoadAndCache(ref driAgentRenamon, "Quests/DriAgents/DriAgentRenamonAddresses.json");
        private QuestAddresses GetDriAgentPatamon() =>
            LoadAndCache(ref driAgentPatamon, "Quests/DriAgents/DriAgentPatamonAddresses.json");

        private QuestAddresses GetSideQuestFolderBag() =>
            LoadAndCache(ref sideQuestFolderBag, "Quests/SideQuests/FolderBagAddresses.json");
        private QuestAddresses GetSideQuestTreeBoots() =>
            LoadAndCache(ref sideQuestTreeBoots, "Quests/SideQuests/TreeBootsAddresses.json");
        private QuestAddresses GetSideQuestFishingPole() =>
            LoadAndCache(ref sideQuestFishingPole, "Quests/SideQuests/FishingPoleAddresses.json");

        private QuestAddresses GetLegendaryWeaponEternally() =>
            LoadAndCache(ref legendaryWeaponEternally, "Quests/LegendaryWeapons/EternallyAddresses.json");
        private QuestAddresses GetLegendaryWeaponInvincible() =>
            LoadAndCache(ref legendaryWeaponInvincible, "Quests/LegendaryWeapons/InvincibleAddresses.json");
        private QuestAddresses GetLegendaryWeaponMuramasa() =>
            LoadAndCache(ref legendaryWeaponMuramasa, "Quests/LegendaryWeapons/MuramasaAddresses.json");
        private QuestAddresses GetLegendaryWeaponSuperNova() =>
            LoadAndCache(ref legendaryWeaponSuperNova, "Quests/LegendaryWeapons/SuperNovaAddresses.json");
        private QuestAddresses GetLegendaryWeaponPunishment() =>
            LoadAndCache(ref legendaryWeaponPunishment, "Quests/LegendaryWeapons/PunishmentAddresses.json");

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

        public List<QuestAddresses> GetAllLegendaryWeapons() =>
        [
            GetLegendaryWeaponEternally(),
            GetLegendaryWeaponInvincible(),
            GetLegendaryWeaponMuramasa(),
            GetLegendaryWeaponSuperNova(),
            GetLegendaryWeaponPunishment()
        ];

        public List<QuestAddresses> GetAllDriAgents() =>
        [
            GetDriAgentGuilmon(),
            GetDriAgentAgumon(),
            GetDriAgentVeemon(),
            GetDriAgentKumamon(),
            GetDriAgentMonmon(),
            GetDriAgentKotemon(),
            GetDriAgentRenamon(),
            GetDriAgentPatamon()
        ];

        public Dictionary<string, AuctionAddresses> GetAuctionAddresses() =>
            LoadAndCache(ref auctionAddresses, "AuctionAddresses.json");
    }
}
