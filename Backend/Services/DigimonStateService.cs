using Backend.Models;
using Backend.Models.Digimons;
using Backend.Models.Addresses;
using Backend.Utils;
using Backend.Interfaces;

namespace Backend.Services
{
    public class DigimonStateService(
        IGameDatabase gameDatabase,
        IGameReader gameReader,
        DigievolutionStateService digievolutionStateService)
    {
        private DigimonAddresses Addresses => gameDatabase.GetDigimonAddresses();

        public bool IsEmptySlot(byte digimonId)
        {
            return digimonId == (byte)Addresses.EmptySlotId;
        }

        public Digimon? GetDigimon(int slotIndex, byte digimonId)
        {
            int baseAddress = (int)(Addresses.Digimons?.FirstOrDefault(d => d.Id == digimonId)?.Address ?? 0);

            if (baseAddress == 0)
            {
                Serilog.Log.Warning("Unknown Digimon ID: 0x{Id:X2}", digimonId);
                return null;
            }

            var resource = gameReader.ReadDigimon(slotIndex, baseAddress, Addresses);
            var digimonName = Addresses.Digimons?.FirstOrDefault(d => d.Id == digimonId)?.Name ?? "Unknown";
            var equippedDigievolutions = digievolutionStateService.GetEquippedDigievolutions(resource.LogicBlock, Addresses.Digievolutions);

            return new Digimon
            {
                SlotIndex = slotIndex,
                ActiveDigievolutionId = resource.ActiveDigievolutionId <= 0 || resource.ActiveDigievolutionId == 65535 ? null : resource.ActiveDigievolutionId,
                BasicInfo = new BasicInfo
                {
                    Name = digimonName,
                    Experience = MemoryUtils.ReadInt32FromBlock(resource.LogicBlock, Addresses.BasicInfo?.Experience ?? 0),
                    Level = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.BasicInfo?.Level ?? 0),
                    CurrentHP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.BasicInfo?.CurrentHP ?? 0),
                    MaxHP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.BasicInfo?.MaxHP ?? 0),
                    CurrentMP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.BasicInfo?.CurrentMP ?? 0),
                    MaxMP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.BasicInfo?.MaxMP ?? 0)
                },
                Attributes = new Attributes
                {
                    Strength = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Attributes?.Strength ?? 0),
                    Defense = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Attributes?.Defense ?? 0),
                    Spirit = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Attributes?.Spirit ?? 0),
                    Wisdom = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Attributes?.Wisdow ?? 0),
                    Speed = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Attributes?.Speed ?? 0),
                    Charisma = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Attributes?.Charisma ?? 0)
                },
                Resistances = new Resistances
                {
                    Fire = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Resistances?.Fire ?? 0),
                    Water = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Resistances?.Water ?? 0),
                    Ice = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Resistances?.Ice ?? 0),
                    Wind = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Resistances?.Wind ?? 0),
                    Thunder = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Resistances?.Thunder ?? 0),
                    Machine = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Resistances?.Machine ?? 0),
                    Dark = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Resistances?.Dark ?? 0)
                },
                Equipments = new Equipments
                {
                    Head = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Equipaments?.Head ?? 0),
                    Body = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Equipaments?.Body ?? 0),
                    RightHand = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Equipaments?.RightHand ?? 0),
                    LeftHand = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Equipaments?.LeftHand ?? 0),
                    Accessory1 = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Equipaments?.Accessory1 ?? 0),
                    Accessory2 = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, Addresses.Equipaments?.Accessory2 ?? 0)
                },
                EquippedDigievolutions = equippedDigievolutions
            };
        }
    }
}
