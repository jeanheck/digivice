using Backend.Models;
using Backend.Models.Digimons;
using Backend.Constants;
using Backend.Interfaces;
using System.Linq;

namespace Backend.Services
{
    public class PartyStateService
    {
        private readonly GameDatabase _database;
        private readonly GameReader _reader;
        private readonly IMemoryReaderService _memoryReader;

        public PartyStateService(GameDatabase database, GameReader reader, IMemoryReaderService memoryReader)
        {
            _database = database;
            _reader = reader;
            _memoryReader = memoryReader;  // We still need memory reader strictly to fetch the complex nested digimon tree properties
        }

        public Party GetParty()
        {
            var addresses = _database.GetPartyAddresses();
            var resource = _reader.ReadParty(addresses);

            var party = new Party();

            for (int i = 0; i < resource.ActiveDigimonIds.Count && i < 3; i++)
            {
                byte digimonId = (byte)resource.ActiveDigimonIds[i];

                if (digimonId == DigimonAddresses.EmptySlotId) continue;

                if (DigimonAddresses.Digimons.TryGetValue(digimonId, out var data))
                {
                    party.Slots[i] = BuildDigimon(i + 1, data, _memoryReader);
                }
                else
                {
                    Serilog.Log.Warning("Unknown Digimon ID detected in party slot: 0x{Id:X2} at address 0x{Address:X8}", digimonId, addresses.PartySlot1);
                }
            }

            return party;
        }

        private Digimon BuildDigimon(int slotIndex, (string Name, int Address) data, IMemoryReaderService memReader)
        {
            var equippedDigievolutions = new Digievolution?[3];
            var equippedEvoIds = new int[]
            {
                memReader.ReadInt16(data.Address + DigimonAddresses.Digievolutions.EquippedSlot1),
                memReader.ReadInt16(data.Address + DigimonAddresses.Digievolutions.EquippedSlot2),
                memReader.ReadInt16(data.Address + DigimonAddresses.Digievolutions.EquippedSlot3)
            };

            for (int j = 0; j < 3; j++)
            {
                int id = equippedEvoIds[j];
                if (id == 0xFFFF || id == -1 || id == 0)
                {
                    equippedDigievolutions[j] = null;
                    continue;
                }

                int level = 1;
                for (int k = 0; k < DigimonAddresses.Digievolutions.MaxUnlockedDigievolutions; k++)
                {
                    int entryAddr = data.Address + DigimonAddresses.Digievolutions.UnlockedDigievolutionsStart + (k * DigimonAddresses.Digievolutions.UnlockedDigievolutionEntryStride);
                    int entryId = memReader.ReadInt16(entryAddr);
                    if (entryId == id)
                    {
                        level = memReader.ReadInt16(entryAddr + 2);
                        break;
                    }
                    if (entryId == 0 || entryId == 0xFFFF || entryId == -1)
                    {
                        break;
                    }
                }

                equippedDigievolutions[j] = new Digievolution { Id = id, Level = level };
            }

            return new Digimon
            {
                SlotIndex = slotIndex,
                BasicInfo = new BasicInfo
                {
                    Name = data.Name,
                    Experience = memReader.ReadInt32(data.Address + DigimonAddresses.BasicInfo.Experience),
                    Level = memReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.Level),
                    CurrentHP = memReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.CurrentHP),
                    MaxHP = memReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.MaxHP),
                    CurrentMP = memReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.CurrentMP),
                    MaxMP = memReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.MaxMP)
                },
                Attributes = new Attributes
                {
                    Strength = memReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Strength),
                    Defense = memReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Defense),
                    Spirit = memReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Spirit),
                    Wisdom = memReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Wisdom),
                    Speed = memReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Speed),
                    Charisma = memReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Charisma)
                },
                Resistances = new Resistances
                {
                    Fire = memReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Fire),
                    Water = memReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Water),
                    Ice = memReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Ice),
                    Wind = memReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Wind),
                    Thunder = memReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Thunder),
                    Machine = memReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Machine),
                    Dark = memReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Dark)
                },
                Equipments = new Equipments
                {
                    Head = memReader.ReadInt16(data.Address + DigimonAddresses.Equipments.Head),
                    Body = memReader.ReadInt16(data.Address + DigimonAddresses.Equipments.Body),
                    RightHand = memReader.ReadInt16(data.Address + DigimonAddresses.Equipments.RightHand),
                    LeftHand = memReader.ReadInt16(data.Address + DigimonAddresses.Equipments.LeftHand),
                    Accessory1 = memReader.ReadInt16(data.Address + DigimonAddresses.Equipments.Accessory1),
                    Accessory2 = memReader.ReadInt16(data.Address + DigimonAddresses.Equipments.Accessory2)
                },
                EquippedDigievolutions = equippedDigievolutions
            };
        }
    }
}
