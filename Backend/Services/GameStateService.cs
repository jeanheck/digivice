using Backend.Interfaces;
using Backend.Models;
using Backend.Models.Digimons;
using Backend.Utils;
using Backend.Constants;

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
                Party = GetParty()
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
                    var equippedEvolutions = new Evolution?[3];
                    var equippedEvoIds = new int[]
                    {
                        _memoryReader.ReadInt16(data.Address + DigimonAddresses.Evolutions.EquippedSlot1),
                        _memoryReader.ReadInt16(data.Address + DigimonAddresses.Evolutions.EquippedSlot2),
                        _memoryReader.ReadInt16(data.Address + DigimonAddresses.Evolutions.EquippedSlot3)
                    };

                    for (int j = 0; j < 3; j++)
                    {
                        int id = equippedEvoIds[j];
                        // Empty slot is usually 0xFFFF (-1) or 0
                        if (id == 0xFFFF || id == -1 || id == 0)
                        {
                            equippedEvolutions[j] = null;
                            continue;
                        }

                        int level = 1;
                        for (int k = 0; k < DigimonAddresses.Evolutions.MaxUnlockedEvolutions; k++)
                        {
                            int entryAddr = data.Address + DigimonAddresses.Evolutions.UnlockedEvolutionsStart + (k * DigimonAddresses.Evolutions.UnlockedEvolutionEntryStride);
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

                        equippedEvolutions[j] = new Evolution { Id = id, Level = level };
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
                        EquippedEvolutions = equippedEvolutions
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
