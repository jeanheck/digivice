using Backend.Domain.Models;
using Backend.Memory.Repositories;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Digimon;
using Backend.Domain.Models.Digimons;
using Backend.Memory.Addresses.Digimon;

namespace Backend.Application.Services
{
    public class DigimonStateService(
        IAddressesRepository addressesRepository,
        IDigimonReader digimonReader,
        DigievolutionStateService digievolutionStateService)
    {
        private const int NoActiveDigievolution = 0xFFFF;

        private DigimonStatusAddresses Addresses => addressesRepository.GetDigimonStatusAddresses();

        public Digimon? GetDigimon(int slotIndex, byte digimonId)
        {
            var (experience, level, vitals, attributes, resistances, equipaments, digievolutions) = Addresses;

            var digimonEntry = addressesRepository.GetDigimonAddressById(digimonId);

            if (digimonEntry == null || digimonEntry.Address == 0)
            {
                Serilog.Log.Warning("Unknown Digimon ID: 0x{Id:X2}", digimonId);
                return null;
            }

            int digimonAddress = (int)digimonEntry.Address;

            var (logicBlock, activeDigievolutionId) = digimonReader
                .Read(slotIndex, digimonAddress, digievolutions);

            var memoryBlockReader = new MemoryBlockReader(logicBlock);

            var equippedDigievolutions = digievolutionStateService
                .GetDigievolutions(memoryBlockReader, digievolutions);

            return new Digimon
            {
                SlotIndex = slotIndex,
                ActiveDigievolutionId = GetActiveDigievolutionId(activeDigievolutionId),
                BasicInfo = new BasicInfo
                {
                    Experience = memoryBlockReader.ReadInt32(experience),
                    Level = memoryBlockReader.ReadInt16(level),
                    CurrentHP = memoryBlockReader.ReadInt16(vitals.CurrentHP),
                    MaxHP = memoryBlockReader.ReadInt16(vitals.MaxHP),
                    CurrentMP = memoryBlockReader.ReadInt16(vitals.CurrentMP),
                    MaxMP = memoryBlockReader.ReadInt16(vitals.MaxMP)
                },
                Attributes = new Attributes
                {
                    Strength = memoryBlockReader.ReadInt16(attributes.Strength),
                    Defense = memoryBlockReader.ReadInt16(attributes.Defense),
                    Spirit = memoryBlockReader.ReadInt16(attributes.Spirit),
                    Wisdom = memoryBlockReader.ReadInt16(attributes.Wisdow),
                    Speed = memoryBlockReader.ReadInt16(attributes.Speed),
                    Charisma = memoryBlockReader.ReadInt16(attributes.Charisma)
                },
                Resistances = new Resistances
                {
                    Fire = memoryBlockReader.ReadInt16(resistances.Fire),
                    Water = memoryBlockReader.ReadInt16(resistances.Water),
                    Ice = memoryBlockReader.ReadInt16(resistances.Ice),
                    Wind = memoryBlockReader.ReadInt16(resistances.Wind),
                    Thunder = memoryBlockReader.ReadInt16(resistances.Thunder),
                    Machine = memoryBlockReader.ReadInt16(resistances.Machine),
                    Dark = memoryBlockReader.ReadInt16(resistances.Dark)
                },
                Equipments = new Equipments
                {
                    Head = memoryBlockReader.ReadInt16(equipaments.Head),
                    Body = memoryBlockReader.ReadInt16(equipaments.Body),
                    RightHand = memoryBlockReader.ReadInt16(equipaments.RightHand),
                    LeftHand = memoryBlockReader.ReadInt16(equipaments.LeftHand),
                    Accessory1 = memoryBlockReader.ReadInt16(equipaments.Accessory1),
                    Accessory2 = memoryBlockReader.ReadInt16(equipaments.Accessory2)
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
