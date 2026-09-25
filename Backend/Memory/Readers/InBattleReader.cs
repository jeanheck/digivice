using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;
using Backend.Memory.Resources.Parties.Digimons;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Memory.Readers
{
    public class InBattleReader(IMemoryReader memoryReader) : IInBattleReader
    {
        public InBattleResource Read(InBattleAddresses addresses, int zeroBasedPartySlotIndex)
        {
            var slotBase = addresses.AllySlotBase + (zeroBasedPartySlotIndex * addresses.SlotStride);

            return new InBattleResource
            {
                Condition = memoryReader.ReadBytes(slotBase + addresses.Condition, 1) is { Length: > 0 } conditionBytes
                    ? conditionBytes[0]
                    : 0,
                Strength = memoryReader.ReadInt16(slotBase + addresses.Strength),
                Defense = memoryReader.ReadInt16(slotBase + addresses.Defense),
                Speed = memoryReader.ReadInt16(slotBase + addresses.Speed),
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
