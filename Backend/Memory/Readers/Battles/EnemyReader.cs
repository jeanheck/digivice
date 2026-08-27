using Backend.Memory.Addresses;
using Backend.Memory.Resources.Battles;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Battles
{
    public class EnemyReader(IMemoryReader memoryReader) : IEnemyReader
    {
        public EnemyResource Read(EnemyAddresses addresses)
        {
            var slotIndex = ResolveActiveSlotIndex(addresses);
            var slotBase = addresses.EnemySlotBase + (slotIndex * addresses.SlotStride);

            return ReadSlot(slotBase, addresses);
        }

        private int ResolveActiveSlotIndex(EnemyAddresses addresses)
        {
            var activeUnitId = memoryReader.ReadInt16(addresses.ActiveUnitId);

            for (var slotIndex = 0; slotIndex < addresses.SlotCount; slotIndex++)
            {
                var slotBase = addresses.EnemySlotBase + (slotIndex * addresses.SlotStride);
                var slotId = memoryReader.ReadInt16(slotBase + addresses.Id);

                if (slotId == activeUnitId && slotId != 0)
                {
                    var currentHp = memoryReader.ReadInt16(slotBase + addresses.HP.Current);
                    if (currentHp > 0)
                    {
                        return slotIndex;
                    }
                }
            }

            for (var slotIndex = 0; slotIndex < addresses.SlotCount; slotIndex++)
            {
                var slotBase = addresses.EnemySlotBase + (slotIndex * addresses.SlotStride);
                var slotId = memoryReader.ReadInt16(slotBase + addresses.Id);
                var currentHp = memoryReader.ReadInt16(slotBase + addresses.HP.Current);

                if (slotId != 0 && currentHp > 0)
                {
                    return slotIndex;
                }
            }

            return 0;
        }

        private EnemyResource ReadSlot(long slotBase, EnemyAddresses addresses)
        {
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
