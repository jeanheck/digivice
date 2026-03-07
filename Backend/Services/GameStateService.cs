using Backend.Interfaces;
using Backend.Models;
using Backend.Models.Digimons;
using Backend.Utils;
using Backend.Constants;
using Backend.Constants.Quests;
using Backend.Constants.Quests.SideQuests;
using Backend.Models.Quests;
using Backend.DetailedQuests;
using Backend.DetailedQuests.SideQuests;

namespace Backend.Services
{
    public class GameStateService
    {
        private readonly IMemoryReaderService _memoryReader;
        private readonly PlayerStateService _playerStateService;
        private readonly PartyStateService _partyStateService;

        public GameStateService(IMemoryReaderService memoryReader, PlayerStateService playerStateService, PartyStateService partyStateService)
        {
            _memoryReader = memoryReader;
            _playerStateService = playerStateService;
            _partyStateService = partyStateService;
        }

        public State GetState()
        {
            return new State
            {
                Player = _playerStateService.GetPlayer(),
                Party = _partyStateService.GetParty(),
                ImportantItems = GetImportantItems(),
                Journal = GetJournal()
            };
        }



        private Dictionary<string, bool> GetImportantItems()
        {
            var items = new Dictionary<string, bool>();
            foreach (var kvp in ImportantItemsAddresses.Items)
            {
                // Key items memory flags are usually 1 byte booleans (0 or 1)
                var bytes = _memoryReader.ReadBytes(kvp.Value, 1);
                items[kvp.Key] = bytes != null && bytes.Length > 0 && bytes[0] == 1;
            }
            return items;
        }

        private Journal GetJournal()
        {
            var journal = new Journal();

            // --- Main Quest ---
            var mainQuest = MainQuestFactory.Get();
            ApplyQuestSteps(mainQuest, MainQuestAddress.Steps);
            journal.MainQuest = mainQuest;

            // --- 1. Folder Bag Side Quest ---
            var folderBag = FolderBag.Get();
            ApplyQuestSteps(folderBag, FolderBagAddress.Steps);
            journal.SideQuests.Add(folderBag);

            // Check if the player owns the Folder Bag (prerequisite for next quests)
            var importantItems = GetImportantItems();
            bool hasFolderBag = importantItems.ContainsKey("FolderBag") && importantItems["FolderBag"];

            // --- 2. Tree Boots Side Quest ---
            var treeBoots = TreeBoots.Get();
            if (treeBoots.Prerequisites.Count > 0)
                treeBoots.Prerequisites[0].IsDone = hasFolderBag;
            ApplyQuestSteps(treeBoots, TreeBootsAddress.Steps);
            journal.SideQuests.Add(treeBoots);

            // --- 3. Fishing Pole Side Quest ---
            var fishingPole = FishingPole.Get();
            if (fishingPole.Prerequisites.Count > 0)
                fishingPole.Prerequisites[0].IsDone = hasFolderBag;
            ApplyQuestSteps(fishingPole, FishingPoleAddress.Steps);
            ApplyStepPrerequisites(fishingPole, importantItems);
            journal.SideQuests.Add(fishingPole);

            return journal;
        }

        /// <summary>
        /// Reads memory and marks quest steps as completed.
        /// Supports two modes:
        ///   - BitMask set: checks if (byte AND mask) != 0 (bit field mode)
        ///   - BitMask null: checks if byte == 1 (legacy mode, e.g., FolderBag)
        /// </summary>
        private void ApplyQuestSteps(Quest quest, List<QuestAddressStep> memSteps)
        {
            foreach (var memStep in memSteps)
            {
                if (memStep.Address == 0x00000000) continue;

                var bytes = _memoryReader.ReadBytes(memStep.Address, 1);
                if (bytes == null || bytes.Length == 0) continue;

                bool isStepDone;
                if (memStep.BitMask.HasValue)
                {
                    // Bitmask mode: check if any bit in the mask is set
                    isStepDone = (bytes[0] & memStep.BitMask.Value) != 0;
                }
                else
                {
                    // Legacy mode: byte == 1
                    isStepDone = bytes[0] == 1;
                }

                var qStep = quest.Steps.FirstOrDefault(s => s.Number == memStep.Id);
                if (qStep != null)
                {
                    qStep.IsCompleted = isStepDone;
                }
            }
        }

        /// <summary>
        /// Reads consumable item quantities from RAM.
        /// Returns a dictionary of item key → quantity owned.
        /// </summary>
        private Dictionary<string, int> GetConsumableItems()
        {
            var items = new Dictionary<string, int>();
            foreach (var kvp in ConsumableItemsAddresses.Items)
            {
                if (kvp.Value == 0x00000000) continue; // Skip mocked addresses
                var bytes = _memoryReader.ReadBytes(kvp.Value, 1);
                items[kvp.Key] = (bytes != null && bytes.Length > 0) ? bytes[0] : 0;
            }
            return items;
        }

        /// <summary>
        /// Checks step-level prerequisites by looking up item ownership.
        /// Supports "consumable" items (quantity > 0) via ConsumableItemsAddresses
        /// and "important" items (flag == 1) via ImportantItemsAddresses.
        /// Prerequisites without an ItemKey are left unchanged.
        /// </summary>
        private void ApplyStepPrerequisites(Quest quest, Dictionary<string, bool> importantItems)
        {
            var consumables = GetConsumableItems();

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
                            // "equipment" can be added later
                    }
                }
            }
        }


    }
}
