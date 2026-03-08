using System;
using Backend.Models;
using Backend.Models.Digimons;
using Backend.Models.Addresses;
using Backend.Models.Resources;
using Backend.Interfaces;
using System.Linq;

namespace Backend.Services
{
    public class DigimonStateService
    {
        private readonly GameDatabase _database;
        private readonly GameReader _reader;
        // GameReader fetches generic Resources, but parsing the Digimon properties
        // demands a tight mapping. We use DigimonResource which has a memory logic block.
        // It's effectively parsed here in the Service layer to create the final Digimon Model.

        public DigimonStateService(GameDatabase database, GameReader reader)
        {
            _database = database;
            _reader = reader;
        }

        public Digimon BuildDigimon(int slotIndex, byte digimonId, int baseAddress)
        {
            var addresses = _database.GetDigimonAddresses();
            var resource = _reader.ReadDigimonResource(slotIndex, digimonId, baseAddress);

            var digimonName = GetDigimonNameById(addresses, digimonId);

            var equippedEvoIds = new int[]
            {
                ReadInt16FromBlock(resource.LogicBlock, addresses.Digievolutions?.EquipedSlot1),
                ReadInt16FromBlock(resource.LogicBlock, addresses.Digievolutions?.EquipedSlot2),
                ReadInt16FromBlock(resource.LogicBlock, addresses.Digievolutions?.EquipedSlot3)
            };

            var equippedDigievolutions = new Digievolution?[3];
            for (int j = 0; j < 3; j++)
            {
                int id = equippedEvoIds[j];
                if (id == 0xFFFF || id == -1 || id == 0)
                {
                    equippedDigievolutions[j] = null;
                    continue;
                }

                int level = 1;
                int maxEvolutions = ReadInt32OffsetSafely(addresses.Digievolutions?.MaxUnlockedDigievolutions, 60);
                int stride = ReadInt32OffsetSafely(addresses.Digievolutions?.UnlockedDigievolutionEntryStride, 8);
                int startOffset = ReadInt32OffsetSafely(addresses.Digievolutions?.UnlockedDigievolutionsStart, 0x50);

                for (int k = 0; k < maxEvolutions; k++)
                {
                    int entryOffset = startOffset + (k * stride);
                    if (entryOffset + 2 >= resource.LogicBlock.Length) break;

                    int entryId = BitConverter.ToInt16(resource.LogicBlock, entryOffset);
                    if (entryId == id)
                    {
                        level = BitConverter.ToInt16(resource.LogicBlock, entryOffset + 2);
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
                    Name = digimonName,
                    Experience = ReadInt32FromBlock(resource.LogicBlock, addresses.BasicInfo?.Experience),
                    Level = ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.Level),
                    CurrentHP = ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.CurrentHP),
                    MaxHP = ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.MaxHP),
                    CurrentMP = ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.CurrentMP),
                    MaxMP = ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.MaxMP)
                },
                Attributes = new Attributes
                {
                    Strength = ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Strength),
                    Defense = ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Defense),
                    Spirit = ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Spirit),
                    Wisdom = ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Wisdow),
                    Speed = ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Speed),
                    Charisma = ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Charisma)
                },
                Resistances = new Resistances
                {
                    Fire = ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Fire),
                    Water = ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Water),
                    Ice = ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Ice),
                    Wind = ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Wind),
                    Thunder = ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Thunder),
                    Machine = ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Machine),
                    Dark = ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Dark)
                },
                Equipments = new Equipments
                {
                    Head = ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.Head),
                    Body = ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.Body),
                    RightHand = ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.RightHand),
                    LeftHand = ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.LeftHand),
                    Accessory1 = ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.Accessory1),
                    Accessory2 = ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.Accessory2)
                },
                EquippedDigievolutions = equippedDigievolutions
            };
        }

        private string GetDigimonNameById(DigimonAddresses addresses, byte id)
        {
            if (addresses.Kotemon?.Id == id) return "Kotemon";
            if (addresses.Kumamon?.Id == id) return "Kumamon";
            if (addresses.Monmon?.Id == id) return "Monmon";
            if (addresses.Agumon?.Id == id) return "Agumon";
            if (addresses.Veemon?.Id == id) return "Veemon";
            if (addresses.Guilmon?.Id == id) return "Guilmon";
            if (addresses.Renamon?.Id == id) return "Renamon";
            if (addresses.Patamon?.Id == id) return "Patamon";
            return "Unknown";
        }

        private int ReadInt32OffsetSafely(string? hexOffset, int fallback)
        {
            if (string.IsNullOrEmpty(hexOffset)) return fallback;
            try
            {
                // Note: The json has integer strings for these sizes/strides instead of hex
                if (int.TryParse(hexOffset, out int num)) return num;
                return Convert.ToInt32(hexOffset, 16);
            }
            catch { return fallback; }
        }

        private short ReadInt16FromBlock(byte[] block, string? hexOffset)
        {
            if (string.IsNullOrEmpty(hexOffset) || block.Length == 0) return 0;
            try
            {
                int offset = Convert.ToInt32(hexOffset, 16);
                if (offset + 1 >= block.Length) return 0;
                return BitConverter.ToInt16(block, offset);
            }
            catch { return 0; }
        }

        private int ReadInt32FromBlock(byte[] block, string? hexOffset)
        {
            if (string.IsNullOrEmpty(hexOffset) || block.Length == 0) return 0;
            try
            {
                int offset = Convert.ToInt32(hexOffset, 16);
                if (offset + 3 >= block.Length) return 0;
                return BitConverter.ToInt32(block, offset);
            }
            catch { return 0; }
        }
    }
}
