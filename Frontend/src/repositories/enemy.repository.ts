import EnemyJson from "@/database/enemy.json";
import type { EnemyTable } from "@/repositories/tables/enemy/enemy.table";
import type { DropType } from "@/repositories/tables/raws/drop/drop-type";
import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";
import type { EnemyWhoDropsRaw } from "@/repositories/tables/raws/enemy/enemy-who-drops.raw";

export class EnemyRepository {
  private static readonly enemyTable = EnemyJson as EnemyTable;
  private static readonly enemiesWhoDropByDropKey = this.buildEnemiesWhoDropByDropKey();

  private static buildEnemiesWhoDropByDropKey(): Map<string, EnemyWhoDropsRaw[]> {
    const enemiesWhoDropByDropKey = new Map<string, EnemyWhoDropsRaw[]>();
    for (const [enemyId, enemyRaw] of Object.entries(this.enemyTable)) {
      for (const dropRaw of enemyRaw.drops ?? []) {
        const dropKey = this.toDropKey(dropRaw.type, dropRaw.dropId);
        const enemiesWhoDrop = enemiesWhoDropByDropKey.get(dropKey) ?? [];
        enemiesWhoDrop.push({ enemyId, locationOnly: dropRaw.locationOnly });
        enemiesWhoDropByDropKey.set(dropKey, enemiesWhoDrop);
      }
    }

    return enemiesWhoDropByDropKey;
  }

  private static toDropKey(dropType: DropType, dropId: number): string {
    return `${dropType}:${dropId}`;
  }

  public static getEnemyById(enemyId: string): EnemyRaw {
    return this.enemyTable[enemyId]!;
  }

  public static getEnemyByMemoryId(memoryId: number): EnemyRaw | null {
    if (memoryId === 0) {
      return null;
    }

    for (const enemyRaw of Object.values(this.enemyTable)) {
      if (enemyRaw.memoryId === memoryId) {
        return enemyRaw;
      }
    }

    return null;
  }

  public static getEnemyByMemoryIdAndGroupId(memoryId: number, groupId: number): EnemyRaw | null {
    if (memoryId === 0) {
      return null;
    }

    for (const enemyRaw of Object.values(this.enemyTable)) {
      if (enemyRaw.memoryId === memoryId && enemyRaw.groupId === groupId) {
        return enemyRaw;
      }
    }

    return this.getEnemyByMemoryId(memoryId);
  }

  public static getEnemyIdByMemoryId(memoryId: number): string | null {
    if (memoryId === 0) {
      return null;
    }

    for (const [enemyId, enemyRaw] of Object.entries(this.enemyTable)) {
      if (enemyRaw.memoryId === memoryId) {
        return enemyId;
      }
    }

    return null;
  }

  public static getEnemyIdByMemoryIdAndGroupId(memoryId: number, groupId: number): string | null {
    if (memoryId === 0) {
      return null;
    }

    for (const [enemyId, enemyRaw] of Object.entries(this.enemyTable)) {
      if (enemyRaw.memoryId === memoryId && enemyRaw.groupId === groupId) {
        return enemyId;
      }
    }

    return this.getEnemyIdByMemoryId(memoryId);
  }

  public static getEnemyTable(): EnemyTable {
    return this.enemyTable;
  }

  public static getEnemiesWhoDropByDrop(dropType: DropType, dropId: number): EnemyWhoDropsRaw[] {
    return this.enemiesWhoDropByDropKey.get(this.toDropKey(dropType, dropId)) ?? [];
  }
}
