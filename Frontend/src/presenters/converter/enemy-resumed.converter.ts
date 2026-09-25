import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";
import type { EnemyResumedViewModel } from "@/viewmodels/enemy/enemy-resumed.viewmodel";

export class EnemyResumedConverter {
  public static convert(
    enemyId: string,
    enemyRaw: EnemyRaw,
    origins: { walking: boolean; fishing: boolean; kickingTree: boolean },
  ): EnemyResumedViewModel {
    return {
      id: enemyId,
      name: enemyRaw.name,
      boss: enemyRaw.boss === true,
      walking: origins.walking,
      fishing: origins.fishing,
      kickingTree: origins.kickingTree,
    };
  }
}
