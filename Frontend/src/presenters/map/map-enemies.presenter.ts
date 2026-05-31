import { EnemyRepository } from "@/repositories/enemy.repository";
import type { EnemyResumedViewModel } from "@/viewmodels/enemy/enemy-resumed.viewmodel";

export class MapEnemiesPresenter {
  public static getResumedEnemiesByIds(enemyIds: string[]): EnemyResumedViewModel[] {
    return enemyIds.map((enemyId) => {
      const enemy = EnemyRepository.getEnemyById(enemyId);

      return {
        id: enemyId,
        name: enemy.name,
        boss: enemy.boss,
      };
    });
  }
}
