import type { Quest } from "@/models";
import { MapRepository } from "@/repositories";
import { MapService } from "@/services/map.service";
import { QuestService } from "@/services/quest.service";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";
import { LocationConverter } from "@/presenters/converter/location.converter";

export class MobiusDesertButtonPresenter {
  public static getLocation(locationId: string, mainQuest: Quest): LocationViewModel {
    const locationRaw = MapRepository.getMapById(locationId);
    const walkingIds = MapService.getWalkingEnemies(
      locationId,
      QuestService.getLastCompletedMainQuestStep(mainQuest),
    );
    const bossIds = MapService.getBoss(locationId);
    const enemyIds = [...bossIds, ...walkingIds];

    return LocationConverter.convert(locationId, locationRaw, enemyIds);
  }
}
