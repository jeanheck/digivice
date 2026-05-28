import EnemyJson from "@/database/enemy/enemy.json";
import type { EnemyTable } from "@/repositories/tables/enemy/enemy.table";
import type { EnemyResumedRaw } from "@/repositories/tables/raws/enemy/enemy-resumed.raw";
import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";

export class EnemyRepository {
  private static readonly enemyTable = EnemyJson as EnemyTable;

  public static getResumedEnemyById(enemyId: string): EnemyResumedRaw {
    const enemyRaw = this.enemyTable[enemyId]!;

    return {
      id: enemyId,
      name: enemyRaw.name,
      boss: enemyRaw.boss ?? false,
    };
  }

  public static getEnemyById(enemyId: string): EnemyRaw {
    return this.enemyTable[enemyId]!;
  }
}
