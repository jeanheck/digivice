import type { Quest } from "@/models";
import { QuestHelper } from "@/presenters/helper/quest.helper";
import { LocationRepository } from "@/repositories/location.repository";
import { SeabedRoutesRepository } from "@/repositories/seabed-routes.repository";
import {
  isLocationEnemyPhaseList,
  type LocationEnemiesRaw,
} from "@/repositories/tables/raws/location/location.raw";

export class LocationService {
  private static readonly asukaSewersLocationId = "021B";
  private static readonly undergroundPathLocationId = "020B";

  private static readonly seabedLocationIds: ReadonlySet<string> = new Set([
    "02E0",
    "02E1",
    "02E2",
    "02E3",
    "02E4",
    "02E5",
    "02E6",
    "02E7",
  ]);

  private static isAsukaSewersSafeZone(locationId: string, previousMapId: string): boolean {
    return (
      locationId === this.asukaSewersLocationId && previousMapId === this.undergroundPathLocationId
    );
  }

  private static resolveSeabedEnemyIds(seabedRoute: number): string[] {
    if (seabedRoute === 0) {
      return [];
    }

    return SeabedRoutesRepository.getEnemiesByRoute(String(seabedRoute));
  }

  private static resolveEnemyIds(
    enemies: LocationEnemiesRaw,
    lastCompletedMainQuestStep: number,
  ): string[] {
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

  public static getEnemies(
    locationId: string,
    mainQuest: Quest | null,
    seabedRoute: number = 0,
    previousMapId: string = "",
  ): string[] {
    const locationRaw = LocationRepository.getLocationById(locationId);

    if (this.isAsukaSewersSafeZone(locationId, previousMapId)) {
      return [];
    }

    if (this.isSeabedLocation(locationId)) {
      return this.resolveSeabedEnemyIds(seabedRoute);
    }

    return this.resolveEnemyIds(
      locationRaw.enemies,
      QuestHelper.getLastCompletedMainQuestStep(mainQuest),
    );
  }

  public static isSeabedLocation(locationId: string | null): boolean {
    if (locationId === null) {
      return false;
    }

    return this.seabedLocationIds.has(locationId);
  }
}
