import type { Quest } from "@/models";
import { LocationRepository } from "@/repositories";
import { LocationService } from "@/services/location.service";
import { QuestService } from "@/services/quest.service";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";
import { LocationConverter } from "../converter/location.converter";

export class MobiusDesertButtonPresenter {
  public static getLocation(locationId: string, mainQuest: Quest | null): LocationViewModel {
    const locationRaw = LocationRepository.getLocationById(locationId);
    const walkingIds = LocationService.getEnemies(
      locationId,
      QuestService.getLastCompletedMainQuestStep(mainQuest),
    );
    const bossIds = LocationService.getBoss(locationId);
    const enemyIds = [...bossIds, ...walkingIds];

    return LocationConverter.convert(locationId, locationRaw, enemyIds);
  }
}
