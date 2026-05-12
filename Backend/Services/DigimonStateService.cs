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
        private const int NoActiveDigievolution = 0xFFFF;
        private const string UnknownDigimonName = "Unknown";

        private DigimonAddresses Addresses => gameDatabase.GetDigimonAddresses();

        public bool IsEmptySlot(byte digimonId)
        {
            return digimonId == (byte)Addresses.EmptySlotId;
        }

        public Digimon? GetDigimon(int slotIndex, byte digimonId)
        {
            var addresses = Addresses;
            var digimonEntry = addresses.Digimons?.FirstOrDefault(d => d.Id == digimonId);

            if (digimonEntry == null || digimonEntry.Address == 0)
            {
                Serilog.Log.Warning("Unknown Digimon ID: 0x{Id:X2}", digimonId);
                return null;
            }

            int digimonAddress = (int)digimonEntry.Address;
            var resource = gameReader.ReadDigimon(slotIndex, digimonAddress, addresses);
            var equippedDigievolutions = digievolutionStateService
                .GetEquippedDigievolutions(resource.LogicBlock, addresses.Digievolutions);

            return new Digimon
            {
                SlotIndex = slotIndex,
                ActiveDigievolutionId = GetActiveDigievolutionId(resource.ActiveDigievolutionId),
                BasicInfo = new BasicInfo
                {
                    Name = digimonEntry.Name ?? UnknownDigimonName,
                    Experience = MemoryUtils.ReadInt32FromBlock(resource.LogicBlock, addresses.BasicInfo.Experience),
                    Level = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo.Level),
                    CurrentHP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo.CurrentHP),
                    MaxHP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo.MaxHP),
                    CurrentMP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo.CurrentMP),
                    MaxMP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo.MaxMP)
                },
                Attributes = new Attributes
                {
                    Strength = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes.Strength),
                    Defense = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes.Defense),
                    Spirit = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes.Spirit),
                    Wisdom = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes.Wisdow),
                    Speed = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes.Speed),
                    Charisma = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes.Charisma)
                },
                Resistances = new Resistances
                {
                    Fire = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances.Fire),
                    Water = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances.Water),
                    Ice = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances.Ice),
                    Wind = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances.Wind),
                    Thunder = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances.Thunder),
                    Machine = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances.Machine),
                    Dark = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances.Dark)
                },
                Equipments = new Equipments
                {
                    Head = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments.Head),
                    Body = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments.Body),
                    RightHand = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments.RightHand),
                    LeftHand = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments.LeftHand),
                    Accessory1 = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments.Accessory1),
                    Accessory2 = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments.Accessory2)
                },
                EquippedDigievolutions = equippedDigievolutions
            };
        }

        private static int? GetActiveDigievolutionId(int id)
        {
            return id <= 0 || id == NoActiveDigievolution ? null : id;
        }
    }
}

