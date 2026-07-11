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
  private static readonly seabedLocationIds: ReadonlySet<string> = new Set([
    "02E0",
    "02E1",
    "02E2",
    "02E3",
    "02E6",
  ]);

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

  public static getLocationById(locationId: string, mainQuest: Quest | null): LocationViewModel {
    const locationRaw = LocationRepository.getLocationById(locationId);
    const lastCompletedMainQuestStep = QuestHelper.getLastCompletedMainQuestStep(mainQuest);
    const enemyIds = MapPresenter.resolveEnemyIds(locationRaw.enemies, lastCompletedMainQuestStep);

    return LocationConverter.convert(locationId, locationRaw, enemyIds);
  }

  public static isSeabedLocation(locationId: string | null): boolean {
    if (locationId === null) {
      return false;
    }

    return MapPresenter.seabedLocationIds.has(locationId);
  }
}
