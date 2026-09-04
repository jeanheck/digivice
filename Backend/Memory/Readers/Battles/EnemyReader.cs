using Backend.Memory.Addresses;
using Backend.Memory.Resources.Battles;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Battles
{
    public class EnemyReader(IMemoryReader memoryReader) : IEnemyReader
    {
        public EnemyResource Read(EnemyAddresses addresses)
        {
            var slotIndex = ResolveActiveEnemySlotIndex(addresses);
            var slotBase = addresses.EnemySlotBase + (slotIndex * addresses.SlotStride);

            return new EnemyResource
            {
                Id = memoryReader.ReadInt16(slotBase + addresses.Id),
                GroupId = memoryReader.ReadInt16(addresses.GroupId),
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

        private int ResolveActiveEnemySlotIndex(EnemyAddresses addresses)
        {
            var activeUnitId = memoryReader.ReadInt16(addresses.ActiveUnitId);

            for (var slotIndex = 0; slotIndex < addresses.SlotCount; slotIndex++)
            {
                var slotBase = addresses.EnemySlotBase + (slotIndex * addresses.SlotStride);
                var slotId = memoryReader.ReadInt16(slotBase + addresses.Id);

                if (slotId == activeUnitId && slotId != 0)
                {
                    return slotIndex;
                }
            }

            var activeEnemySlotIndex = memoryReader.ReadInt16(addresses.ActiveEnemySlotIndex);

            if (activeEnemySlotIndex >= 0 && activeEnemySlotIndex < addresses.SlotCount)
            {
                var activeSlotBase = addresses.EnemySlotBase + (activeEnemySlotIndex * addresses.SlotStride);
                var activeSlotId = memoryReader.ReadInt16(activeSlotBase + addresses.Id);

                if (activeSlotId != 0)
                {
                    return activeEnemySlotIndex;
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

            for (var slotIndex = addresses.SlotCount - 1; slotIndex >= 0; slotIndex--)
            {
                var slotBase = addresses.EnemySlotBase + (slotIndex * addresses.SlotStride);
                var slotId = memoryReader.ReadInt16(slotBase + addresses.Id);

                if (slotId != 0)
                {
                    return slotIndex;
                }
            }

            return 0;
        }
    }
}
