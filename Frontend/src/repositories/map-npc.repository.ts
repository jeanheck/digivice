import { MapRepository } from "@/repositories/map.repository";
import { NpcRepository } from "@/repositories/npc.repository";
import type { MapNpcRaw } from "@/repositories/tables/raws/map/map-npc.raw";
import { QuestService } from "@/services/quest.service";

export class MapNpcRepository {
  public static getByMapId(mapId: string): MapNpcRaw[] {
    return MapRepository.getMapById(mapId).npcs ?? [];
  }

  public static getIdsByMapId(
    mapId: string,
    lastCompletedMainQuestStep: number,
  ): string[] {
    return this.getByMapId(mapId).flatMap((mapNpcRaw) => {
      if (
        !QuestService.isOnMainQuestRange(
          lastCompletedMainQuestStep,
          NpcRepository.getNpcById(mapNpcRaw.id)?.mainQuestAvailabilityWindow,
        )
      ) {
        return [];
      }

      return [mapNpcRaw.id];
    });
  }
}
