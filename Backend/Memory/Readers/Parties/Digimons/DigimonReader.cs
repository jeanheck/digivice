using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Parties.Digimons
{
    public class DigimonReader(
        IMemoryReader memoryReader,
        IDigievolutionSlotReader digievolutionSlotReader,
        IDigievolutionReader digievolutionReader,
        IStoredDigievolutionReader storedDigievolutionReader) : IDigimonReader
    {
        private const int DigimonMemoryBlockSize = 1500;

        public DigimonResource? Read(DigimonAddress digimonAddress, DigimonStatusAddresses digimonStatusAddresses)
        {
            var memoryBlock = memoryReader.ReadBytes(digimonAddress.Address, DigimonMemoryBlockSize);

            if (memoryBlock.Length < DigimonMemoryBlockSize)
            {
                return null;
            }

            var memoryBlockReader = new MemoryBlockReader(memoryBlock);

            var digievolutionsSlots = digimonStatusAddresses.Digievolutions.Slots
                .Select(slot => digievolutionSlotReader.Read(memoryBlockReader, slot))
                .ToList();
            foreach (var digievolutionSlot in digievolutionsSlots)
            {
                if (digievolutionSlot.DigievolutionId is not null)
                {
                    digievolutionSlot.DigievolutionResource = digievolutionReader
                        .Read(memoryBlockReader, digievolutionSlot.DigievolutionId.Value, digimonStatusAddresses.Digievolutions);
                }
                else
                {
                    digievolutionSlot.DigievolutionResource = null;
                }
            }

            var activeDigievolutionId = memoryReader.ReadInt16(digimonAddress.Address + digimonStatusAddresses.Digievolutions.ActiveDigievolution);

            return new DigimonResource
            {
                ActiveDigievolutionId = activeDigievolutionId,
                Experience = memoryBlockReader.ReadInt32(digimonStatusAddresses.Experience),
                Level = memoryBlockReader.ReadInt16(digimonStatusAddresses.Level),
                TP = memoryBlockReader.ReadInt16(digimonStatusAddresses.TP),
                BlastGauge = memoryReader.ReadInt16(digimonAddress.BlastGaugeAddress),
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
                    Right = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipaments.Right),
                    Left = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipaments.Left),
                    Accessory1 = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipaments.Accessory1),
                    Accessory2 = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipaments.Accessory2)
                },
                Digievolutions = digievolutionsSlots,
                StoredDigievolutions = storedDigievolutionReader.Read(
                    memoryBlockReader,
                    digimonStatusAddresses.Digievolutions)
            };
        }
    }
}
