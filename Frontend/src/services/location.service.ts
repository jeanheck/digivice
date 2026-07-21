import { LocationRegionConstant } from "@/constants/location-region.constant";
import type { Quest } from "@/models";
import { LocationRepository } from "@/repositories/location.repository";
import { SeabedRoutesRepository } from "@/repositories/seabed-routes.repository";
import { isLocationEnemyPhaseList } from "@/repositories/tables/raws/location/location.raw";
import { QuestService } from "@/services/quest.service";

export class LocationService {
  private static readonly ASUKA_SEWERS_LOCATION_ID = "021B";
  private static readonly UNDERGROUND_PATH_LOCATION_ID = "020B";

  private static isAsukaSewersSafeZone(locationId: string, previousMapId: string): boolean {
    return locationId === this.ASUKA_SEWERS_LOCATION_ID && previousMapId === this.UNDERGROUND_PATH_LOCATION_ID;
  }

  private static getSeabedEnemies(seabedRoute: number): string[] {
    return seabedRoute === 0 ? [] : SeabedRoutesRepository.getEnemiesByRoute(String(seabedRoute));
  }

  private static getEnemies(locationId: string, lastCompletedMainQuestStep: number): string[] {
    const locationRaw = LocationRepository.getLocationById(locationId);
    const locationEnemiesRaw = locationRaw.enemies;

    if (!isLocationEnemyPhaseList(locationEnemiesRaw)) {
      return locationEnemiesRaw;
    }

    const sortedPhases = [...locationEnemiesRaw].sort((firstPhase, secondPhase) => {
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

  public static getRegionByLocationId(id: string | null): LocationRegionConstant {
    if (id === null) {
      return LocationRegionConstant.asukaServer;
    }

    return LocationRepository.getLocationById(id).region ?? LocationRegionConstant.asukaServer;
  }

  public static getLocationImageNameByLocationId(id: string | null): string | null {
    if (id === null) {
      return null;
    }

    return LocationRepository.getLocationById(id).imageName;
  }

  public static getCurrentEnemies(
    locationId: string,
    mainQuest: Quest | null,
    seabedRoute: number = 0,
    previousMapId: string = "",
  ): string[] {
    if (this.isAsukaSewersSafeZone(locationId, previousMapId)) {
      return [];
    }
    if (this.isSeabed(locationId)) {
      return this.getSeabedEnemies(seabedRoute);
    }

    return this.getEnemies(locationId, QuestService.getLastCompletedMainQuestStep(mainQuest));
  }

  public static isSeabed(locationId: string | null): boolean {
    return this.getRegionByLocationId(locationId) === LocationRegionConstant.seabed;
  }
}
