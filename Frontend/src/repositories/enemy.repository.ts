import EnemyJson from "@/database/enemy/enemy.json";
import type { EnemyTable } from "./tables/enemy/enemy.table";
import type { EnemyResumedViewModel } from "@/viewmodels/enemy-resumed.viewmodel";
import type { EnemyViewModel } from "@/viewmodels/enemy.viewmodel";

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

    public static getEnemyById(enemyId: string): EnemyViewModel {
        return this.enemyTable[enemyId]!;
    }
}
