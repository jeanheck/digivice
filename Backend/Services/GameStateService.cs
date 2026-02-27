using Backend.Interfaces;
using Backend.Models;
using Backend.Models.Digimons;
using Backend.Utils;
using Backend.Constants;
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
                Player = GetPlayer(),
                Party = GetParty(),
                ImportantItems = GetImportantItems(),
                Journal = GetJournal()
            };
        }

        private Player? GetPlayer()
        {
            var bytes = _memoryReader.ReadBytes(PlayerAddresses.Name, PlayerAddresses.NameBufferSize);
            if (bytes == null) return null;

            var player = new Player
            {
                Name = TextDecoder.Decode(bytes),
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

            foreach (var memStep in FolderBagAddress.Steps)
            {
                var bytes = _memoryReader.ReadBytes(memStep.Address, 1);
                bool isStepDone = bytes != null && bytes.Length > 0 && bytes[0] == 1;

                var qStep = folderBag.Steps.FirstOrDefault(s => s.Number == memStep.Id);
                if (qStep != null)
                {
                    qStep.IsCompleted = isStepDone;
                }
            }

            journal.SideQuests.Add(folderBag);

            return journal;
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
