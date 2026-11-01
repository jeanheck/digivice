import { MapRepository } from "@/presentation/repositories/map.repository";
import type { MapBossRaw } from "@/presentation/repositories/tables/raws/map/map-boss.raw";

export class MapBossRepository {
  public static getByMapId(mapId: string): MapBossRaw[] {
    return MapRepository.getMapById(mapId).enemies?.boss ?? [];
  }
}
