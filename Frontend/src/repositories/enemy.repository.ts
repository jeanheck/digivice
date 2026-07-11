import EnemyJson from "@/database/enemy/enemy.json";
import type { EnemyTable } from "@/repositories/tables/enemy/enemy.table";
import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";

export class EnemyRepository {
  private static readonly enemyTable = EnemyJson as EnemyTable;

  public static getEnemyById(enemyId: string): EnemyRaw {
    return this.enemyTable[enemyId]!;
  }

  public static getEnemyTable(): EnemyTable {
    return this.enemyTable;
  }
}
