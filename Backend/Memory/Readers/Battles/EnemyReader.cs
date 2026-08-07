using Backend.Memory.Addresses;
using Backend.Memory.Resources.Battles;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Battles
{
    public class EnemyReader(IMemoryReader memoryReader) : IEnemyReader
    {
        public EnemyResource Read(BattleAddresses addresses)
        {
            var slotBase = addresses.EnemySlotBase;

            return new EnemyResource
            {
                Id = memoryReader.ReadInt16(slotBase + addresses.Id),
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
                }
            };
        }
    }
}
