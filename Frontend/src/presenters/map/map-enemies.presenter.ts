import { EnemyRepository } from "@/repositories/enemy.repository";
import { EnemyResumedConverter } from "@/presenters/converter/enemy-resumed.converter";
import type { EnemyResumedViewModel } from "@/viewmodels/enemy/enemy-resumed.viewmodel";

export class MapEnemiesPresenter {
  public static getResumedEnemiesByIds(enemyIds: string[]): EnemyResumedViewModel[] {
    return this.getResumedEnemiesByEncounterSources(enemyIds, [], []);
  }

  public static getResumedEnemiesByEncounterSources(
    walkIds: string[],
    fishingIds: string[],
    kickingTreesIds: string[],
  ): EnemyResumedViewModel[] {
    const originsByEnemyId = new Map<
      string,
      { walk: boolean; fishing: boolean; kickingTrees: boolean }
    >();
    const orderedEnemyIds: string[] = [];

    const registerOrigin = (
      enemyId: string,
      originKey: "walk" | "fishing" | "kickingTrees",
    ): void => {
      const existingOrigins = originsByEnemyId.get(enemyId);
      if (existingOrigins === undefined) {
        originsByEnemyId.set(enemyId, {
          walk: originKey === "walk",
          fishing: originKey === "fishing",
          kickingTrees: originKey === "kickingTrees",
        });
        orderedEnemyIds.push(enemyId);
        return;
      }

      existingOrigins[originKey] = true;
    };

    for (const enemyId of walkIds) {
      registerOrigin(enemyId, "walk");
    }
    for (const enemyId of fishingIds) {
      registerOrigin(enemyId, "fishing");
    }
    for (const enemyId of kickingTreesIds) {
      registerOrigin(enemyId, "kickingTrees");
    }

    return orderedEnemyIds.map((enemyId) => {
      const enemyRaw = EnemyRepository.getEnemyById(enemyId);
      const origins = originsByEnemyId.get(enemyId)!;

      return EnemyResumedConverter.convert(enemyId, enemyRaw, origins);
    });
  }
}
