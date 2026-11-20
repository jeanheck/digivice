import type { Journal } from "@/state/models";
import { MapRepository } from "@/presentation/repositories";
import { MapService } from "@/presentation/services/map.service";
import { QuestService } from "@/presentation/services/quest.service";
import type { LocationViewModel } from "@/presentation/viewmodels/location/location.viewmodel";
import { LocationConverter } from "@/presentation/presenters/converter/location.converter";

export class MobiusDesertButtonPresenter {
  public static getLocation(locationId: string, journal: Journal): LocationViewModel {
    const locationRaw = MapRepository.getMapById(locationId);
    const walkingIds = MapService.getWalkingEnemies(
      locationId,
      QuestService.getLastCompletedMainQuestStep(journal.mainQuest),
    );
    const bossIds = MapService.getBoss(locationId, journal);
    const enemyIds = [...bossIds, ...walkingIds];

    return LocationConverter.convert(locationId, locationRaw, enemyIds);
  }
}
