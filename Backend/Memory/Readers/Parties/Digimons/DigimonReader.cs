using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Parties.Digimons
{
    public class DigimonReader(
        IMemoryReader memoryReader,
        IDigievolutionSlotReader digievolutionSlotReader,
        IDigievolutionReader digievolutionReader,
        IStoredDigievolutionReader storedDigievolutionReader,
        IInBattleReader digimonInBattleReader) : IDigimonReader
    {
        private const int DigimonMemoryBlockSize = 1500;

        public DigimonResource? Read(
            DigimonAddress digimonAddress,
            DigimonStatusAddresses digimonStatusAddresses,
            InBattleAddresses inBattleAddresses,
            int zeroBasedPartySlotIndex)
        {
            var memoryBlock = memoryReader.ReadBytes(digimonAddress.MemoryBlockAddress, DigimonMemoryBlockSize);

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

            var activeDigievolutionId = memoryReader.ReadInt16(digimonAddress.MemoryBlockAddress + digimonStatusAddresses.Digievolutions.ActiveDigievolution);

            return new DigimonResource
            {
                ActiveDigievolutionId = activeDigievolutionId,
                Experience = memoryBlockReader.ReadInt32(digimonStatusAddresses.Experience),
                Level = memoryBlockReader.ReadInt16(digimonStatusAddresses.Level),
                TP = memoryBlockReader.ReadInt16(digimonStatusAddresses.TP),
                Blast = memoryReader.ReadInt16(digimonAddress.BlastAddress),
                HP = new VitalResource
                {
                    Current = memoryBlockReader.ReadInt16(digimonStatusAddresses.HP.Current),
                    Max = memoryBlockReader.ReadInt16(digimonStatusAddresses.HP.Max)
                },
                MP = new VitalResource
                {
                    Current = memoryBlockReader.ReadInt16(digimonStatusAddresses.MP.Current),
                    Max = memoryBlockReader.ReadInt16(digimonStatusAddresses.MP.Max)
                },
                InBattle = digimonInBattleReader.Read(inBattleAddresses, zeroBasedPartySlotIndex),
                Attributes = new AttributesResource
                {
                    Strength = memoryBlockReader.ReadInt16(digimonStatusAddresses.Attributes.Strength),
                    Defense = memoryBlockReader.ReadInt16(digimonStatusAddresses.Attributes.Defense),
                    Spirit = memoryBlockReader.ReadInt16(digimonStatusAddresses.Attributes.Spirit),
                    Wisdom = memoryBlockReader.ReadInt16(digimonStatusAddresses.Attributes.Wisdom),
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
                    Head = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipments.Head),
                    Body = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipments.Body),
                    Right = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipments.Right),
                    Left = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipments.Left),
                    Accessory1 = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipments.Accessory1),
                    Accessory2 = memoryBlockReader.ReadInt16(digimonStatusAddresses.Equipments.Accessory2)
                },
                Digievolutions = digievolutionsSlots,
                StoredDigievolutions = storedDigievolutionReader.Read(
                    memoryBlockReader,
                    digimonStatusAddresses.Digievolutions)
            };
        }
    }
}
