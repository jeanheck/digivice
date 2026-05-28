import { EnemyConverter } from "@/converters/enemy/enemy.converter";
import { EnemyResumedConverter } from "@/converters/enemy/enemy-resumed.converter";
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
      const enemyResumedRaw = EnemyRepository.getResumedEnemyById(enemyId);
      return EnemyResumedConverter.convert(enemyResumedRaw);
    });
  }

  public static getEnemyById(enemyId: string): EnemyViewModel {
    const enemyRaw = EnemyRepository.getEnemyById(enemyId);
    return EnemyConverter.convert(enemyRaw);
  }
}
