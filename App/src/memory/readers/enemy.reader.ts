import type { EnemyAddresses } from "@/memory/addresses/enemy.addresses";
import type { EnemyResource } from "@/memory/resources/battles/enemy.resource";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class EnemyReader {
  private static getSlotBase(addresses: EnemyAddresses, slotIndex: number): number {
    return addresses.enemySlotBase + slotIndex * addresses.slotStride;
  }

  private static readSlotId(
    snapshot: MemorySnapshot,
    addresses: EnemyAddresses,
    slotIndex: number,
  ): number {
    return snapshot.readInt16(EnemyReader.getSlotBase(addresses, slotIndex) + addresses.id);
  }

  private static resolveActiveEnemySlotIndex(
    snapshot: MemorySnapshot,
    addresses: EnemyAddresses,
  ): number {
    const activeEnemySlotIndex = snapshot.readInt16(addresses.activeEnemySlotIndex);
    if (
      activeEnemySlotIndex >= 0 &&
      activeEnemySlotIndex < addresses.slotCount &&
      EnemyReader.readSlotId(snapshot, addresses, activeEnemySlotIndex) !== 0
    ) {
      return activeEnemySlotIndex;
    }

    for (let slotIndex = 0; slotIndex < addresses.slotCount; slotIndex++) {
      const slotId = EnemyReader.readSlotId(snapshot, addresses, slotIndex);
      const currentHp = snapshot.readInt16(
        EnemyReader.getSlotBase(addresses, slotIndex) + addresses.hp.current,
      );

      if (slotId !== 0 && currentHp > 0) {
        return slotIndex;
      }
    }

    for (let slotIndex = addresses.slotCount - 1; slotIndex >= 0; slotIndex--) {
      if (EnemyReader.readSlotId(snapshot, addresses, slotIndex) !== 0) {
        return slotIndex;
      }
    }

    return 0;
  }

  public static read(snapshot: MemorySnapshot, addresses: EnemyAddresses): EnemyResource {
    const slotIndex = EnemyReader.resolveActiveEnemySlotIndex(snapshot, addresses);
    const slotBase = EnemyReader.getSlotBase(addresses, slotIndex);

    return {
      id: snapshot.readInt16(slotBase + addresses.id),
      groupId: snapshot.readInt16(addresses.groupId),
      condition: snapshot.readByte(slotBase + addresses.condition),
      strength: snapshot.readInt16(slotBase + addresses.strength),
      defense: snapshot.readInt16(slotBase + addresses.defense),
      speed: snapshot.readInt16(slotBase + addresses.speed),
      hp: {
        max: snapshot.readInt16(slotBase + addresses.hp.max),
        current: snapshot.readInt16(slotBase + addresses.hp.current),
      },
    };
  }
}
