import type { MapRegion } from "@/types/map-region.type";
import { MapBossRepository } from "@/repositories/map-boss.repository";
import { MapDuelIslandRepository } from "@/repositories/map-duel-island.repository";
import { MapNpcRepository } from "@/repositories/map-npc.repository";
import { MapRepository } from "@/repositories/map.repository";
import { MapTamerRepository } from "@/repositories/map-tamer.repository";
import { SeabedRoutesRepository } from "@/repositories/seabed-routes.repository";
import type { InnerLocationRaw } from "@/repositories/tables/raws/map/inner-location.raw";
import {
  isMapEnemyPhaseList,
  type MapWalkingEnemiesRaw,
} from "@/repositories/tables/raws/map/map.raw";
import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";

export class MapService {
  public static getSeabedEnemies(seabedRoute: number | null): string[] {
    return seabedRoute === null ? [] : SeabedRoutesRepository.getEnemiesByRoute(String(seabedRoute));
  }

  public static getWalkingEnemies(mapId: string, lastCompletedMainQuestStep: number): string[] {
    const mapRaw = MapRepository.getMapById(mapId);
    return this.resolvePhasedIds(mapRaw.enemies?.walking ?? [], lastCompletedMainQuestStep);
  }

  public static getBoss(mapId: string): string[] {
    return MapBossRepository.getIdsByMapId(mapId);
  }

  public static getFishingEnemies(mapId: string): string[] {
    return MapRepository.getMapById(mapId).enemies?.fishing ?? [];
  }

  public static getKickingTreeEnemies(mapId: string): string[] {
    return MapRepository.getMapById(mapId).enemies?.kickingTree ?? [];
  }

  public static getMapOpponentIds(
    mapId: string,
    lastCompletedMainQuestStep: number,
  ): string[] {
    const npcIds = MapNpcRepository.getIdsByMapId(
      mapId,
      lastCompletedMainQuestStep,
    );
    const tamerIds = MapTamerRepository.getIdsByMapId(mapId);
    const duelIslandIds = MapDuelIslandRepository.getIdsByMapId(mapId);

    return [...new Set([...npcIds, ...tamerIds, ...duelIslandIds])];
  }

  private static resolvePhasedIds(
    phasedIdsRaw: MapWalkingEnemiesRaw,
    lastCompletedMainQuestStep: number,
  ): string[] {
    if (!isMapEnemyPhaseList(phasedIdsRaw)) {
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

  public static getRegionByMapId(id: string): MapRegion {
    return MapRepository.getMapById(id).region ?? "asukaServer";
  }

  public static getImageNameByMapId(id: string): string | null {
    return MapRepository.getMapById(id).imageName;
  }

  public static getWorldLocation(mapId: string): CoordinatesRaw | undefined {
    return MapRepository.getMapById(mapId).worldLocation;
  }

  public static getInnerLocation(mapId: string): InnerLocationRaw[] {
    return MapRepository.getMapById(mapId).innerLocation ?? [];
  }
}
