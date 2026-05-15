using Backend.Models;
using Backend.Models.Digimons;
using Backend.Addresses;
using Backend.Addresses.Digimon;
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
        private PartyAddresses PartyAddresses => gameDatabase.GetPartyAddresses();

        public bool IsEmptySlot(byte digimonId)
        {
            return digimonId == (byte)PartyAddresses.EmptySlotId;
        }

        public Digimon? GetDigimon(int slotIndex, byte digimonId)
        {
            var (basicInfo, attributes, resistances, equipaments, digievolutions) = Addresses;

            if (!gameDatabase.GetDigimonDefinitions().TryGetValue(digimonId, out var digimonEntry) || digimonEntry.Address == 0)
            {
                Serilog.Log.Warning("Unknown Digimon ID: 0x{Id:X2}", digimonId);
                return null;
            }

            int digimonAddress = (int)digimonEntry.Address;

            var (logicBlock, activeDigievolutionId) = gameReader
                .ReadDigimon(slotIndex, digimonAddress, digievolutions);
            var equippedDigievolutions = digievolutionStateService
                .GetDigievolutions(logicBlock, digievolutions);

            return new Digimon
            {
                SlotIndex = slotIndex,
                ActiveDigievolutionId = GetActiveDigievolutionId(activeDigievolutionId),
                BasicInfo = new BasicInfo
                {
                    Name = digimonEntry.Name ?? UnknownDigimonName,
                    Experience = MemoryUtils.ReadInt32FromBlock(logicBlock, basicInfo.Experience),
                    Level = MemoryUtils.ReadInt16FromBlock(logicBlock, basicInfo.Level),
                    CurrentHP = MemoryUtils.ReadInt16FromBlock(logicBlock, basicInfo.CurrentHP),
                    MaxHP = MemoryUtils.ReadInt16FromBlock(logicBlock, basicInfo.MaxHP),
                    CurrentMP = MemoryUtils.ReadInt16FromBlock(logicBlock, basicInfo.CurrentMP),
                    MaxMP = MemoryUtils.ReadInt16FromBlock(logicBlock, basicInfo.MaxMP)
                },
                Attributes = new Attributes
                {
                    Strength = MemoryUtils.ReadInt16FromBlock(logicBlock, attributes.Strength),
                    Defense = MemoryUtils.ReadInt16FromBlock(logicBlock, attributes.Defense),
                    Spirit = MemoryUtils.ReadInt16FromBlock(logicBlock, attributes.Spirit),
                    Wisdom = MemoryUtils.ReadInt16FromBlock(logicBlock, attributes.Wisdow),
                    Speed = MemoryUtils.ReadInt16FromBlock(logicBlock, attributes.Speed),
                    Charisma = MemoryUtils.ReadInt16FromBlock(logicBlock, attributes.Charisma)
                },
                Resistances = new Resistances
                {
                    Fire = MemoryUtils.ReadInt16FromBlock(logicBlock, resistances.Fire),
                    Water = MemoryUtils.ReadInt16FromBlock(logicBlock, resistances.Water),
                    Ice = MemoryUtils.ReadInt16FromBlock(logicBlock, resistances.Ice),
                    Wind = MemoryUtils.ReadInt16FromBlock(logicBlock, resistances.Wind),
                    Thunder = MemoryUtils.ReadInt16FromBlock(logicBlock, resistances.Thunder),
                    Machine = MemoryUtils.ReadInt16FromBlock(logicBlock, resistances.Machine),
                    Dark = MemoryUtils.ReadInt16FromBlock(logicBlock, resistances.Dark)
                },
                Equipments = new Equipments
                {
                    Head = MemoryUtils.ReadInt16FromBlock(logicBlock, equipaments.Head),
                    Body = MemoryUtils.ReadInt16FromBlock(logicBlock, equipaments.Body),
                    RightHand = MemoryUtils.ReadInt16FromBlock(logicBlock, equipaments.RightHand),
                    LeftHand = MemoryUtils.ReadInt16FromBlock(logicBlock, equipaments.LeftHand),
                    Accessory1 = MemoryUtils.ReadInt16FromBlock(logicBlock, equipaments.Accessory1),
                    Accessory2 = MemoryUtils.ReadInt16FromBlock(logicBlock, equipaments.Accessory2)
                },
                Digievolutions = equippedDigievolutions
            };
        }

        private static int? GetActiveDigievolutionId(int id)
        {
            return id <= 0 || id == NoActiveDigievolution ? null : id;
        }
    }
}

