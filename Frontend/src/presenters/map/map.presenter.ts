import { LocationConverter } from "@/presenters/converter/location.converter";
import { QuestHelper } from "@/presenters/helper/quest.helper";
import type { Quest } from "@/models";
import { LocationRepository } from "@/repositories/location.repository";
import {
    isLocationEnemyPhaseList,
    type LocationEnemiesRaw,
} from "@/repositories/tables/raws/location/location.raw";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class MapPresenter {
  public static getLocationById(locationId: string, mainQuest: Quest | null): LocationViewModel {
    const locationRaw = LocationRepository.getLocationById(locationId);
    const lastCompletedMainQuestStep = QuestHelper.getLastCompletedMainQuestStep(mainQuest);
    const enemyIds = MapPresenter.resolveEnemyIds(locationRaw.enemies, lastCompletedMainQuestStep);

    return LocationConverter.convert(locationId, locationRaw, enemyIds);
  }

  private static resolveEnemyIds(enemies: LocationEnemiesRaw, lastCompletedMainQuestStep: number): string[] {
    if (!isLocationEnemyPhaseList(enemies)) {
      return enemies;
    }

    const sortedPhases = [...enemies].sort((firstPhase, secondPhase) => {
      return secondPhase.lastMainQuestStepDone - firstPhase.lastMainQuestStepDone;
    });

    const matchingPhase = sortedPhases.find((phase) => {
      return lastCompletedMainQuestStep >= phase.lastMainQuestStepDone;
    });

    if (matchingPhase === undefined) {
      return [];
    }

    return matchingPhase.ids;
  }
}
