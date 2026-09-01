import { LocationRegionConstant } from "@/constants/location-region.constant";
import { LocationDuelIslandRepository } from "@/repositories/location-duel-island.repository";
import { LocationNpcRepository } from "@/repositories/location-npc.repository";
import { LocationRepository } from "@/repositories/location.repository";
import { LocationTamerRepository } from "@/repositories/location-tamer.repository";
import { SeabedRoutesRepository } from "@/repositories/seabed-routes.repository";
import type { InnerLocationRaw } from "@/repositories/tables/raws/location/inner-location.raw";
import {
  isLocationEnemyPhaseList,
  type LocationWalkingEnemiesRaw,
} from "@/repositories/tables/raws/location/location.raw";
import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";

export class LocationService {
  public static getSeabedEnemies(seabedRoute: number): string[] {
    return seabedRoute === 0 ? [] : SeabedRoutesRepository.getEnemiesByRoute(String(seabedRoute));
  }

  public static getEnemies(locationId: string, lastCompletedMainQuestStep: number): string[] {
    const locationRaw = LocationRepository.getLocationById(locationId);
    return this.resolvePhasedIds(locationRaw.enemies?.walking ?? [], lastCompletedMainQuestStep);
  }

  public static getBoss(locationId: string): string[] {
    return LocationRepository.getLocationById(locationId).enemies?.boss ?? [];
  }

  public static getFishing(locationId: string): string[] {
    return LocationRepository.getLocationById(locationId).enemies?.fishing ?? [];
  }

  public static getKickingTree(locationId: string): string[] {
    return LocationRepository.getLocationById(locationId).enemies?.kickingTree ?? [];
  }

  public static getMapOpponentIds(
    locationId: string,
    lastCompletedMainQuestStep: number,
  ): string[] {
    const npcIds = LocationNpcRepository.getIdsByLocationId(
      locationId,
      lastCompletedMainQuestStep,
    );
    const tamerIds = LocationTamerRepository.getIdsByLocationId(locationId);
    const duelIslandIds = LocationDuelIslandRepository.getIdsByLocationId(locationId);

    return [...new Set([...npcIds, ...tamerIds, ...duelIslandIds])];
  }

  private static resolvePhasedIds(
    phasedIdsRaw: LocationWalkingEnemiesRaw,
    lastCompletedMainQuestStep: number,
  ): string[] {
    if (!isLocationEnemyPhaseList(phasedIdsRaw)) {
      return phasedIdsRaw;
    }

    const sortedPhases = [...phasedIdsRaw].sort((firstPhase, secondPhase) => {
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

  public static getWorldLocation(locationId: string): CoordinatesRaw | undefined {
    return LocationRepository.getLocationById(locationId).worldLocation;
  }

  public static getInnerLocation(locationId: string): InnerLocationRaw[] {
    return LocationRepository.getLocationById(locationId).innerLocation ?? [];
  }
}
