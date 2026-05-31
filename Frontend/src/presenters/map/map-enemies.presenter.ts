import { EnemyRepository } from "@/repositories/enemy.repository";
import { EnemyResumedConverter } from "@/presenters/converter/enemy-resumed.converter";
import type { EnemyResumedViewModel } from "@/viewmodels/enemy/enemy-resumed.viewmodel";

export class MapEnemiesPresenter {
  public static getResumedEnemiesByIds(enemyIds: string[]): EnemyResumedViewModel[] {
    return enemyIds.map((enemyId) => {
      const enemyRaw = EnemyRepository.getEnemyById(enemyId);

      return EnemyResumedConverter.convert(enemyId, enemyRaw);
    });
  }
}
