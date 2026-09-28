import { MapRepository } from "@/repositories/map.repository";
import type { MapBossRaw } from "@/repositories/tables/raws/map/map-boss.raw";

export class MapBossRepository {
  public static getByMapId(mapId: string): MapBossRaw[] {
    return MapRepository.getMapById(mapId).enemies?.boss ?? [];
  }

  public static getIdsByMapId(mapId: string): string[] {
    return this.getByMapId(mapId).map((mapBossRaw) => {
      return mapBossRaw.id;
    });
  }
}
