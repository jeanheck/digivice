import EnemyJson from "@/database/enemy/enemy.json";
import type { EnemyTable } from "@/repositories/tables/enemy/enemy.table";
import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";

export class EnemyRepository {
  private static readonly enemyTable = EnemyJson as EnemyTable;

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
}
