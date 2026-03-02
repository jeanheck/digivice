using Backend.Interfaces;
using Backend.Models;
using Backend.Models.Digimons;
using Backend.Utils;
using Backend.Constants;
using Backend.Constants.Quests;
using Backend.Constants.Quests.SideQuests;
using Backend.Models.Quests;
using Backend.DetailedQuests.SideQuests;

namespace Backend.Services
{
    public class GameStateService
    {
        private readonly IMemoryReaderService _memoryReader;

        public GameStateService(IMemoryReaderService memoryReader)
        {
            _memoryReader = memoryReader;
        }

        public State GetState()
        {
            return new State
            {
                CurrentLocation = GetCurrentLocation(),
                Player = GetPlayer(),
                Party = GetParty(),
                ImportantItems = GetImportantItems(),
                Journal = GetJournal()
            };
        }

        private string GetCurrentLocation()
        {
            short rawMapId = _memoryReader.ReadInt16(LocationAddress.MapIdAddress);
            if (rawMapId < 0) return "Unknown";

            return rawMapId.ToString("X4");
        }

        private Player? GetPlayer()
        {
            var bytes = _memoryReader.ReadBytes(PlayerAddresses.Name, PlayerAddresses.NameBufferSize);
            if (bytes == null) return null;

            var player = new Player
            {
                Name = TextDecoder.DecodeName(bytes),
                Bits = _memoryReader.ReadInt32(PlayerAddresses.Bits)
            };

            return player;
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

            // --- Main Quest Placeholder ---
            journal.MainQuest = new MainQuest
            {
                Id = 0,
                Title = "Unknown Objective",
                Description = "Awaiting destination."
            };

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

        private Party GetParty()
        {
            var party = new Party();

            for (int i = 0; i < PlayerAddresses.PartySlots.Length; i++)
            {
                var slotAddress = PlayerAddresses.PartySlots[i];
                // Each slot ID is an Int32, but we only need the first byte
                var idBytes = _memoryReader.ReadBytes(slotAddress, PlayerAddresses.PartySlotStride);
                if (idBytes == null) continue;

                byte digimonId = idBytes[0];

                if (digimonId == DigimonAddresses.EmptySlotId) continue; // Skip empty slots

                if (DigimonAddresses.Digimons.TryGetValue(digimonId, out var data))
                {
                    var equippedDigievolutions = new Digievolution?[3];
                    var equippedEvoIds = new int[]
                    {
                        _memoryReader.ReadInt16(data.Address + DigimonAddresses.Digievolutions.EquippedSlot1),
                        _memoryReader.ReadInt16(data.Address + DigimonAddresses.Digievolutions.EquippedSlot2),
                        _memoryReader.ReadInt16(data.Address + DigimonAddresses.Digievolutions.EquippedSlot3)
                    };

                    for (int j = 0; j < 3; j++)
                    {
                        int id = equippedEvoIds[j];
                        // Empty slot is usually 0xFFFF (-1) or 0
                        if (id == 0xFFFF || id == -1 || id == 0)
                        {
                            equippedDigievolutions[j] = null;
                            continue;
                        }

                        int level = 1;
                        for (int k = 0; k < DigimonAddresses.Digievolutions.MaxUnlockedDigievolutions; k++)
                        {
                            int entryAddr = data.Address + DigimonAddresses.Digievolutions.UnlockedDigievolutionsStart + (k * DigimonAddresses.Digievolutions.UnlockedDigievolutionEntryStride);
                            int entryId = _memoryReader.ReadInt16(entryAddr);
                            if (entryId == id)
                            {
                                level = _memoryReader.ReadInt16(entryAddr + 2);
                                break;
                            }
                            if (entryId == 0 || entryId == 0xFFFF || entryId == -1)
                            {
                                // Hit end of unlocked list without finding the level, stop scanning.
                                break;
                            }
                        }

                        equippedDigievolutions[j] = new Digievolution { Id = id, Level = level };
                    }

                    party.Slots[i] = new Digimon
                    {
                        SlotIndex = i + 1,
                        BasicInfo = new BasicInfo
                        {
                            Name = data.Name,
                            Experience = _memoryReader.ReadInt32(data.Address + DigimonAddresses.BasicInfo.Experience),
                            Level = _memoryReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.Level),
                            CurrentHP = _memoryReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.CurrentHP),
                            MaxHP = _memoryReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.MaxHP),
                            CurrentMP = _memoryReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.CurrentMP),
                            MaxMP = _memoryReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.MaxMP)
                        },
                        Attributes = new Attributes
                        {
                            Strength = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Strength),
                            Defense = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Defense),
                            Spirit = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Spirit),
                            Wisdom = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Wisdom),
                            Speed = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Speed),
                            Charisma = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Charisma)
                        },
                        Resistances = new Resistances
                        {
                            Fire = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Fire),
                            Water = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Water),
                            Ice = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Ice),
                            Wind = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Wind),
                            Thunder = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Thunder),
                            Machine = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Machine),
                            Dark = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Dark)
                        },
                        Equipments = new Equipments
                        {
                            Head = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Equipments.Head),
                            Body = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Equipments.Body),
                            RightHand = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Equipments.RightHand),
                            LeftHand = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Equipments.LeftHand),
                            Accessory1 = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Equipments.Accessory1),
                            Accessory2 = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Equipments.Accessory2)
                        },
                        EquippedDigievolutions = equippedDigievolutions
                    };
                }
                else
                {
                    Serilog.Log.Warning("Unknown Digimon ID detected in party slot: 0x{Id:X2} at address 0x{Address:X8}", digimonId, slotAddress);
                }
            }

            return party;
        }
    }
}
