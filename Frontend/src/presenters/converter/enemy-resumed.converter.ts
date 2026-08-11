import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";
import type { EnemyResumedViewModel } from "@/viewmodels/enemy/enemy-resumed.viewmodel";

export class EnemyResumedConverter {
  public static convert(
    enemyId: string,
    enemyRaw: EnemyRaw,
    origins: { walk: boolean; fishing: boolean; kickingTrees: boolean },
  ): EnemyResumedViewModel {
    return {
      id: enemyId,
      name: enemyRaw.name,
      boss: enemyRaw.boss,
      walk: origins.walk,
      fishing: origins.fishing,
      kickingTrees: origins.kickingTrees,
    };
  }
}
