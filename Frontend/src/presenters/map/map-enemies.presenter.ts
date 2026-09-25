import { EnemyRepository } from "@/repositories/enemy.repository";
import { EnemyResumedConverter } from "@/presenters/converter/enemy-resumed.converter";
import type { EnemyResumedViewModel } from "@/viewmodels/enemy/enemy-resumed.viewmodel";

export class MapEnemiesPresenter {
  public static getResumedEnemiesByIds(enemyIds: string[]): EnemyResumedViewModel[] {
    return this.getResumedEnemiesByEncounterSources(enemyIds, [], [], []);
  }

  public static getResumedEnemiesByEncounterSources(
    walkingIds: string[],
    bossIds: string[],
    fishingIds: string[],
    kickingTreeIds: string[],
  ): EnemyResumedViewModel[] {
    const originsByEnemyId = new Map<
      string,
      { walking: boolean; fishing: boolean; kickingTree: boolean }
    >();
    const orderedEnemyIds: string[] = [];

    const ensureEnemy = (enemyId: string): void => {
      if (originsByEnemyId.has(enemyId)) {
        return;
      }

      originsByEnemyId.set(enemyId, {
        walking: false,
        fishing: false,
        kickingTree: false,
      });
      orderedEnemyIds.push(enemyId);
    };

    const registerOrigin = (
      enemyId: string,
      originKey: "walking" | "fishing" | "kickingTree",
    ): void => {
      ensureEnemy(enemyId);
      originsByEnemyId.get(enemyId)![originKey] = true;
    };

    for (const enemyId of bossIds) {
      ensureEnemy(enemyId);
    }
    for (const enemyId of walkingIds) {
      registerOrigin(enemyId, "walking");
    }
    for (const enemyId of fishingIds) {
      registerOrigin(enemyId, "fishing");
    }
    for (const enemyId of kickingTreeIds) {
      registerOrigin(enemyId, "kickingTree");
    }

    return orderedEnemyIds.map((enemyId) => {
      const enemyRaw = EnemyRepository.getEnemyById(enemyId);
      const origins = originsByEnemyId.get(enemyId)!;

      return EnemyResumedConverter.convert(enemyId, enemyRaw, origins);
    });
  }
}
