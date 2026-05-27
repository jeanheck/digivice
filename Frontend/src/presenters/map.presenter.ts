import { EnemyRepository } from "@/repositories/enemy.repository";
import { LocationRepository } from "@/repositories/location.repository";
import type { EnemyResumedViewModel } from "@/viewmodels/enemy/enemy-resumed.viewmodel";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class MapPresenter {
    public static getLocationById(locationId: string): LocationViewModel {
        return LocationRepository.getLocationById(locationId);
    }

    public static getResumedEnemiesByIds(enemyIds: string[]): EnemyResumedViewModel[] {
        return enemyIds.map((enemyId) => EnemyRepository.getResumedEnemyById(enemyId));
    }
}