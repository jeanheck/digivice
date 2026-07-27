import { LocationRegionConstant } from "@/constants/location-region.constant";
import { LocationRepository } from "@/repositories/location.repository";
import { SeabedRoutesRepository } from "@/repositories/seabed-routes.repository";
import { isLocationEnemyPhaseList } from "@/repositories/tables/raws/location/location.raw";

export class LocationService {
  public static getSeabedEnemies(seabedRoute: number): string[] {
    return seabedRoute === 0 ? [] : SeabedRoutesRepository.getEnemiesByRoute(String(seabedRoute));
  }

  public static getEnemies(locationId: string, lastCompletedMainQuestStep: number): string[] {
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

  public static isSeabed(locationId: string | null): boolean {
    return this.getRegionByLocationId(locationId) === LocationRegionConstant.seabed;
  }
}
