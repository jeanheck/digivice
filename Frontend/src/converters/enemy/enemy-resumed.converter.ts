import type { EnemyResumedRaw } from "@/repositories/tables/raws/enemy/enemy-resumed.raw";
import type { EnemyResumedViewModel } from "@/viewmodels/enemy/enemy-resumed.viewmodel";

export class EnemyResumedConverter {
  public static convert(enemyResumedRaw: EnemyResumedRaw): EnemyResumedViewModel {
    return {
      id: enemyResumedRaw.id,
      name: enemyResumedRaw.name,
      boss: enemyResumedRaw.boss,
    };
  }
}
