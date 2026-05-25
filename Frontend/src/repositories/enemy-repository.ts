import EnemyJson from "@/database/enemy.json";
import type { EnemyTable } from "./tables/enemy/enemy-table";

export class EnemyRepository {
    private static readonly enemyTable = EnemyJson as EnemyTable;

    public static getEnemiesByLocationAlias(locationAlias: string): null {
        return null;
    }
}
