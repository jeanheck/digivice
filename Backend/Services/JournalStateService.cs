using Backend.Models;
using Backend.Models.Quests;
using Backend.Interfaces;

namespace Backend.Services
{
    public class JournalStateService
    {
        private readonly IGameDatabase _database;
        private readonly IGameReader _reader;
        private readonly ItemsStateService _itemStateService;

        public JournalStateService(IGameDatabase database, IGameReader reader, ItemsStateService itemStateService)
        {
            _database = database;
            _reader = reader;
            _itemStateService = itemStateService;
        }

        public Journal GetJournal()
        {
            var journal = new Journal
            {
                MainQuest = GetMainQuest(),
                SideQuests = GetSideQuests()
            };
            return journal;
        }

        private MainQuest GetMainQuest()
        {
            // --- Main Quest ---
            var mainQuest = _database.GetMainQuest();
            var mainQuestResource = _reader.ReadQuestSteps(mainQuest.Steps);
            ApplyQuestStepsLogic(mainQuest, mainQuest.Steps, mainQuestResource);
            return mainQuest;
        }

        private List<SideQuest> GetSideQuests()
        {
            var sideQuests = new List<SideQuest>();

            // --- 1. Folder Bag Side Quest ---
            var folderBag = _database.GetSideQuestFolderBag();
            var folderBagResource = _reader.ReadQuestSteps(folderBag.Steps);
            ApplyQuestStepsLogic(folderBag, folderBag.Steps, folderBagResource);
            sideQuests.Add(folderBag);

            // Check if the player owns the Folder Bag (prerequisite for next quests)
            var importantItems = _itemStateService.GetImportantItems();
            bool hasFolderBag = importantItems.FolderBag?.Has ?? false;

            // --- 2. Tree Boots Side Quest ---
            var treeBoots = _database.GetSideQuestTreeBoots();
            var treeBootsResource = _reader.ReadQuestSteps(treeBoots.Steps);
            if (treeBoots.Prerequisites.Count > 0)
                treeBoots.Prerequisites[0].IsDone = hasFolderBag;
            ApplyQuestStepsLogic(treeBoots, treeBoots.Steps, treeBootsResource);
            sideQuests.Add(treeBoots);

            // --- 3. Fishing Pole Side Quest ---
            var fishingPole = _database.GetSideQuestFishingPole();
            var fishingPoleResource = _reader.ReadQuestSteps(fishingPole.Steps);
            if (fishingPole.Prerequisites.Count > 0)
                fishingPole.Prerequisites[0].IsDone = hasFolderBag;
            ApplyQuestStepsLogic(fishingPole, fishingPole.Steps, fishingPoleResource);
            ApplyStepPrerequisites(fishingPole, importantItems);
            sideQuests.Add(fishingPole);

            return sideQuests;
        }

        private void ApplyQuestStepsLogic(Quest quest, List<QuestStep> memSteps, Dictionary<int, byte> activeBytes)
        {
            foreach (var memStep in memSteps)
            {
                if (!activeBytes.TryGetValue(memStep.Number, out byte value)) continue;

                bool isStepDone;
                if (!string.IsNullOrEmpty(memStep.BitMask))
                {
                    // Bitmask mode: parse hex string to int, then check if any bit in the mask is set
                    int maskValue = Convert.ToInt32(memStep.BitMask, 16);
                    isStepDone = (value & maskValue) != 0;
                }
                else
                {
                    // Legacy mode: byte == 1
                    isStepDone = value == 1;
                }

                var qStep = quest.Steps.FirstOrDefault(s => s.Number == memStep.Number);
                if (qStep != null)
                {
                    qStep.IsCompleted = isStepDone;
                }
            }

            // Cascata de conclusão: Se o próximo step estiver concluído, o step atual também deve estar.
            // Isso resolve o problema de variáveis temporárias (como a da gôndola) que o jogo reseta posteriormente.
            for (int i = quest.Steps.Count - 2; i >= 0; i--)
            {
                if (!quest.Steps[i].IsCompleted && quest.Steps[i + 1].IsCompleted)
                {
                    quest.Steps[i].IsCompleted = true;
                }
            }
        }

        private void ApplyStepPrerequisites(Quest quest, ImportantItems importantItems)
        {
            var consumables = _itemStateService.GetConsumableItems();

            foreach (var step in quest.Steps)
            {
                if (step.Prerequisites == null) continue;
                foreach (var prereq in step.Prerequisites)
                {
                    if (string.IsNullOrEmpty(prereq.ItemKey) || string.IsNullOrEmpty(prereq.ItemType))
                        continue;

                    switch (prereq.ItemType)
                    {
                        case "consumable":
                            prereq.IsDone = consumables.GetQuantity(prereq.ItemKey) > 0;
                            break;
                        case "important":
                            prereq.IsDone = importantItems.HasItem(prereq.ItemKey);
                            break;
                    }
                }
            }
        }
    }
}
