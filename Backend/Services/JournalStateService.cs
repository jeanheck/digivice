using Backend.Models;
using Backend.Models.Quests;
using Backend.Interfaces;

namespace Backend.Services
{
    public class JournalStateService(
        IGameDatabase gameDatabase,
        IGameReader gameReader,
        ItemsStateService itemStateService)
    {
        public Journal GetJournal()
        {
            return new Journal()
            {
                MainQuest = GetMainQuest(),
                SideQuests = GetSideQuests()
            };
        }

        private MainQuest GetMainQuest()
        {
            var mainQuest = gameDatabase.GetMainQuest();
            var mainQuestStepsState = gameReader.ReadQuestSteps(mainQuest.Steps);
            ApplyQuestStepsLogic(mainQuest, mainQuestStepsState);
            return mainQuest;
        }

        private List<SideQuest> GetSideQuests()
        {
            var sideQuests = new List<SideQuest>();

            // --- 1. Folder Bag Side Quest ---
            var folderBag = gameDatabase.GetSideQuestFolderBag();
            var folderBagResource = gameReader.ReadQuestSteps(folderBag.Steps);
            ApplyQuestStepsLogic(folderBag, folderBagResource);
            sideQuests.Add(folderBag);

            // Check if the player owns the Folder Bag (prerequisite for next quests)
            var importantItems = itemStateService.GetImportantItems();
            bool hasFolderBag = importantItems.FolderBag?.Has ?? false;

            // --- 2. Tree Boots Side Quest ---
            var treeBoots = gameDatabase.GetSideQuestTreeBoots();
            var treeBootsResource = gameReader.ReadQuestSteps(treeBoots.Steps);
            if (treeBoots.Prerequisites.Count > 0)
                treeBoots.Prerequisites[0].IsDone = hasFolderBag;
            ApplyQuestStepsLogic(treeBoots, treeBootsResource);
            sideQuests.Add(treeBoots);

            // --- 3. Fishing Pole Side Quest ---
            var fishingPole = gameDatabase.GetSideQuestFishingPole();
            var fishingPoleResource = gameReader.ReadQuestSteps(fishingPole.Steps);
            if (fishingPole.Prerequisites.Count > 0)
                fishingPole.Prerequisites[0].IsDone = hasFolderBag;
            ApplyQuestStepsLogic(fishingPole, fishingPoleResource);
            ApplyStepPrerequisites(fishingPole, importantItems);
            sideQuests.Add(fishingPole);

            return sideQuests;
        }

        private static void ApplyQuestStepsLogic(Quest quest, Dictionary<int, byte> questStepsState)
        {
            foreach (var step in quest.Steps)
            {
                if (!questStepsState.TryGetValue(step.Number, out byte state))
                {
                    continue;
                }

                step.IsCompleted = state == 1;
                if (!string.IsNullOrEmpty(step.BitMask))
                {
                    // Bitmask mode: parse hex string to int, then check if any bit in the mask is set
                    int maskValue = Convert.ToInt32(step.BitMask, 16);
                    step.IsCompleted = (state & maskValue) != 0;
                }
            }

            // Completion cascade: If the next step is completed, the current step must also be completed.
            // This solves the problem of temporary transfers (like the gondola transfer) where the game restarts afterward.
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
            var consumables = itemStateService.GetConsumableItems();

            foreach (var step in quest.Steps)
            {
                if (step.Prerequisites == null)
                {
                    continue;
                }

                foreach (var prerequisite in step.Prerequisites)
                {
                    switch (prerequisite.ItemType)
                    {
                        case "consumable":
                            prerequisite.IsDone = consumables.GetQuantity(prerequisite.ItemKey) > 0;
                            break;
                        case "important":
                            prerequisite.IsDone = importantItems.HasItem(prerequisite.ItemKey);
                            break;
                    }
                }
            }
        }
    }
}
