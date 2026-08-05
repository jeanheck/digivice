using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Parties
{
    public class InCombatReader(IMemoryReader memoryReader) : IInCombatReader
    {
        public InCombatResource Read(InCombatAddresses addresses, int zeroBasedPartySlotIndex)
        {
            var slotBase = addresses.AllySlotBase + (zeroBasedPartySlotIndex * addresses.SlotStride);

            return new InCombatResource
            {
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
