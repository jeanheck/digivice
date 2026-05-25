import EnemyJson from "@/database/enemy.json";
import type { EnemyTable } from "./tables/enemy/enemy-table";
import type { EnemyResumedViewModel } from "@/view-models/enemy-resumed-view-model";

export class EnemyRepository {
    private static readonly enemyTable = EnemyJson as EnemyTable;

    public static getResumedEnemyById(enemyId: string): EnemyResumedViewModel {
        const enemyRaw = this.enemyTable[enemyId]!;

        return {
            id: enemyId,
            name: enemyRaw.name,
            boss: enemyRaw.boss ?? false,
        };
    }
}
