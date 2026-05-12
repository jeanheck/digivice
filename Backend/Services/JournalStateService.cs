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
            var importantItems = itemStateService.GetImportantItems();
            var consumableItems = itemStateService.GetConsumableItems();

            return new Journal()
            {
                MainQuest = (MainQuest)ProcessQuest(gameDatabase.GetMainQuest(), importantItems, consumableItems),
                SideQuests = gameDatabase.GetAllSideQuests()
                    .Select(sideQuest => (SideQuest)ProcessQuest(sideQuest, importantItems, consumableItems))
                    .ToList()
            };
        }

        private Quest ProcessQuest(Quest quest, ImportantItems importantItems, ConsumableItems consumableItems)
        {
            var questStepsState = gameReader.ReadQuestSteps(quest.Steps);

            // 1. Evaluate Quest-level Prerequisites
            ApplyRequisitesLogic(quest.Prerequisites, importantItems, consumableItems);
            // 2. Evaluate Step Completion (Bitmasks + Cascade)
            ApplyQuestStepsLogic(quest, questStepsState);
            // 3. Evaluate Step-level Prerequisites
            foreach (var step in quest.Steps)
            {
                if (step.Prerequisites != null)
                {
                    ApplyRequisitesLogic(step.Prerequisites, importantItems, consumableItems);
                }
            }

            return quest;
        }

        private static void ApplyQuestStepsLogic(Quest quest, Dictionary<int, byte> questStepsState)
        {
            foreach (var step in quest.Steps)
            {
                if (!questStepsState.TryGetValue(step.Number, out byte state))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(step.BitMask))
                {
                    // Legacy mode: byte == 1 means completed
                    step.IsCompleted = state == 1;
                }
                else
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

        private static void ApplyRequisitesLogic(
            IEnumerable<Requisite> requisites,
            ImportantItems importantItems,
            ConsumableItems consumableItems)
        {
            foreach (var requisite in requisites)
            {
                requisite.IsDone = requisite.ItemType switch
                {
                    "consumable" => consumableItems.GetQuantity(requisite.ItemKey) > 0,
                    "important" => importantItems.HasItem(requisite.ItemKey),
                    _ => requisite.IsDone // Preserve manual state for unknown types
                };
            }
        }
    }
}
