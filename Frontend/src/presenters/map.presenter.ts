import { EnemyRepository } from "@/repositories/enemy.repository";
import { LocationRepository } from "@/repositories/location.repository";
import type { EnemyResumedViewModel } from "@/viewmodels/enemy/enemy-resumed.viewmodel";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class MapPresenter {
  public static getLocationById(locationId: string): LocationViewModel {
    return LocationRepository.getLocationById(locationId);
  }

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

  public static getEnemyById(enemyId: string): EnemyViewModel {
    return EnemyRepository.getEnemyById(enemyId);
  }
}
