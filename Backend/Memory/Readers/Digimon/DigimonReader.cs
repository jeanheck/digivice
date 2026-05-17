using Backend.Memory.Addresses.Party;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Digimon;

namespace Backend.Memory.Readers.Digimon
{
    public class DigimonReader(
        IMemoryReader memoryReader,
        IDigievolutionSlotReader digievolutionSlotReader,
        IDigievolutionReader digievolutionReader) : IDigimonReader
    {
        private const int DigimonMemoryBlockSize = 1500;

        public DigimonResource Read(DigimonAddress digimonAddress, DigimonStatusAddresses digimonStatusAddresses)
        {
            var memoryBlock = memoryReader.ReadBytes(digimonAddress.Address, DigimonMemoryBlockSize)
                ?? throw new InvalidOperationException($"Failed to read memory block for Digimon at address: 0x{digimonAddress.Address:X}");
            var memoryBlockReader = new MemoryBlockReader(memoryBlock);

            var digievolutionsSlots = digimonStatusAddresses.Digievolutions.Slots
                .Select(slot => digievolutionSlotReader.Read(memoryBlockReader, slot))
                .ToList();
            foreach (var digievolutionSlot in digievolutionsSlots)
            {
                digievolutionSlot.DigievolutionResource = digievolutionReader
                    .Read(memoryBlockReader, digievolutionSlot.DigievolutionId, digimonStatusAddresses.Digievolutions);
            }

            var activeDigievolutionId = memoryReader.ReadInt16(digimonAddress.Address + digimonStatusAddresses.Digievolutions.ActiveDigievolution);

            return new DigimonResource
            {
                ActiveDigievolutionId = activeDigievolutionId ?? 0,
                Experience = memoryBlockReader.ReadInt32(digimonStatusAddresses.Experience),
                Level = memoryBlockReader.ReadInt16(digimonStatusAddresses.Level),
                Vitals = new VitalsResource
                {
                    CurrentHP = memoryBlockReader.ReadInt16(digimonStatusAddresses.Vitals.CurrentHP),
                    MaxHP = memoryBlockReader.ReadInt16(digimonStatusAddresses.Vitals.MaxHP),
                    CurrentMP = memoryBlockReader.ReadInt16(digimonStatusAddresses.Vitals.CurrentMP),
                    MaxMP = memoryBlockReader.ReadInt16(digimonStatusAddresses.Vitals.MaxMP)
                },
                Attributes = new AttributesResource
                {
                    Strength = memoryBlockReader.ReadInt16(digimonStatusAddresses.Attributes.Strength),
                    Defense = memoryBlockReader.ReadInt16(digimonStatusAddresses.Attributes.Defense),
                    Spirit = memoryBlockReader.ReadInt16(digimonStatusAddresses.Attributes.Spirit),
                    Wisdow = memoryBlockReader.ReadInt16(digimonStatusAddresses.Attributes.Wisdow),
                    Speed = memoryBlockReader.ReadInt16(digimonStatusAddresses.Attributes.Speed),
                    Charisma = memoryBlockReader.ReadInt16(digimonStatusAddresses.Attributes.Charisma)
                },
                Resistances = new ResistancesResource
                {
                    Fire = memoryBlockReader.ReadInt16(digimonStatusAddresses.Resistances.Fire),
                    Water = memoryBlockReader.ReadInt16(digimonStatusAddresses.Resistances.Water),
                    Ice = memoryBlockReader.ReadInt16(digimonStatusAddresses.Resistances.Ice),
                    Wind = memoryBlockReader.ReadInt16(digimonStatusAddresses.Resistances.Wind),
                    Thunder = memoryBlockReader.ReadInt16(digimonStatusAddresses.Resistances.Thunder),
                    Machine = memoryBlockReader.ReadInt16(digimonStatusAddresses.Resistances.Machine),
                    Dark = memoryBlockReader.ReadInt16(digimonStatusAddresses.Resistances.Dark)
                },
                Equipments = new EquipmentsResource
                {
                    Head = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipaments.Head),
                    Body = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipaments.Body),
                    RightHand = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipaments.RightHand),
                    LeftHand = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipaments.LeftHand),
                    Accessory1 = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipaments.Accessory1),
                    Accessory2 = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipaments.Accessory2)
                },
                Digievolutions = digievolutionsSlots
            };
        }
    }
}
