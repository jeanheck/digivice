using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Parties
{
    public class DigimonInCombatReader(IMemoryReader memoryReader) : IDigimonInCombatReader
    {
        public DigimonInCombatResource Read(DigimonInCombatAddresses addresses, int zeroBasedPartySlotIndex)
        {
            var slotBase = addresses.AllySlotBase + (zeroBasedPartySlotIndex * addresses.SlotStride);

            return new DigimonInCombatResource
            {
                Id = memoryReader.ReadInt16(slotBase + addresses.Id),
                Condition = memoryReader.ReadBytes(slotBase + addresses.Condition, 1) is { Length: > 0 } conditionBytes
                    ? conditionBytes[0]
                    : 0,
                HP = new VitalResource
                {
                    Max = memoryReader.ReadInt16(slotBase + addresses.HP.Max),
                    Current = memoryReader.ReadInt16(slotBase + addresses.HP.Current)
                },
                MP = new VitalResource
                {
                    Max = memoryReader.ReadInt16(slotBase + addresses.MP.Max),
                    Current = memoryReader.ReadInt16(slotBase + addresses.MP.Current)
                }
            };
        }
    }
}
