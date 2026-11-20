import { MapRepository } from "@/presentation/repositories/map.repository";
import type { MapDuelIslandRaw } from "@/presentation/repositories/tables/raws/map/map-duel-island.raw";

export class MapDuelIslandRepository {
  public static getByMapId(mapId: string): MapDuelIslandRaw[] {
    return MapRepository.getMapById(mapId).duelIsland ?? [];
  }

  public static getIdsByMapId(mapId: string): string[] {
    return this.getByMapId(mapId).map((mapDuelIslandRaw) => {
      return mapDuelIslandRaw.id;
    });
  }
}
