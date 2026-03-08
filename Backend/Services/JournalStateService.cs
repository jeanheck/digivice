using Backend.DetailedQuests;
using Backend.DetailedQuests.SideQuests;
using Backend.Models;
using Backend.Models.Quests;

namespace Backend.Services
{
    public class JournalStateService
    {
        private readonly GameDatabase _database;
        private readonly GameReader _reader;
        private readonly ItemsStateService _itemStateService;

        public JournalStateService(GameDatabase database, GameReader reader, ItemsStateService itemStateService)
        {
            _database = database;
            _reader = reader;
            _itemStateService = itemStateService;
        }

        public void ApplyMainQuest(Journal journal)
        {
            // --- Main Quest ---
            var mainQuest = MainQuestFactory.Get();
            var mainQuestResource = _reader.ReadMainQuestSteps(Backend.Constants.Quests.MainQuestAddress.Steps);
            ApplyQuestStepsLogic(mainQuest, Backend.Constants.Quests.MainQuestAddress.Steps, mainQuestResource);
            journal.MainQuest = mainQuest;
        }

        public void ApplySideQuests(Journal journal)
        {
            // --- 1. Folder Bag Side Quest ---
            var folderBagAddress = _database.GetSideQuestAddresses("FolderBag");
            var folderBagResource = _reader.ReadQuestSteps(folderBagAddress);
            var folderBag = FolderBag.Get();
            ApplyQuestStepsLogic(folderBag, folderBagAddress.Steps, folderBagResource);
            journal.SideQuests.Add(folderBag);

            // Check if the player owns the Folder Bag (prerequisite for next quests)
            var importantItems = _itemStateService.GetImportantItems();
            bool hasFolderBag = importantItems.ContainsKey("FolderBag") && importantItems["FolderBag"];

            // --- 2. Tree Boots Side Quest ---
            var treeBootsAddress = _database.GetSideQuestAddresses("TreeBoots");
            var treeBootsResource = _reader.ReadQuestSteps(treeBootsAddress);
            var treeBoots = TreeBoots.Get();
            if (treeBoots.Prerequisites.Count > 0)
                treeBoots.Prerequisites[0].IsDone = hasFolderBag;
            ApplyQuestStepsLogic(treeBoots, treeBootsAddress.Steps, treeBootsResource);
            journal.SideQuests.Add(treeBoots);

            // --- 3. Fishing Pole Side Quest ---
            var fishingPoleAddress = _database.GetSideQuestAddresses("FishingPole");
            var fishingPoleResource = _reader.ReadQuestSteps(fishingPoleAddress);
            var fishingPole = FishingPole.Get();
            if (fishingPole.Prerequisites.Count > 0)
                fishingPole.Prerequisites[0].IsDone = hasFolderBag;
            ApplyQuestStepsLogic(fishingPole, fishingPoleAddress.Steps, fishingPoleResource);
            ApplyStepPrerequisites(fishingPole, importantItems);
            journal.SideQuests.Add(fishingPole);
        }

        private void ApplyQuestStepsLogic(Quest quest, List<QuestAddressStep> memSteps, Dictionary<int, byte> activeBytes)
        {
            foreach (var memStep in memSteps)
            {
                if (!activeBytes.TryGetValue(memStep.Id, out byte value)) continue;

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

                var qStep = quest.Steps.FirstOrDefault(s => s.Number == memStep.Id);
                if (qStep != null)
                {
                    qStep.IsCompleted = isStepDone;
                }
            }
        }

        private void ApplyStepPrerequisites(Quest quest, Dictionary<string, bool> importantItems)
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
                            if (consumables.TryGetValue(prereq.ItemKey, out int qty))
                                prereq.IsDone = qty > 0;
                            break;
                        case "important":
                            if (importantItems.TryGetValue(prereq.ItemKey, out bool owned))
                                prereq.IsDone = owned;
                            break;
                    }
                }
            }
        }
    }
}
