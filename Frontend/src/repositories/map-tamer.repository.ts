import { MapRepository } from "@/repositories/map.repository";
import type { MapTamerRaw } from "@/repositories/tables/raws/map/map-tamer.raw";

export class MapTamerRepository {
  public static getByMapId(mapId: string): MapTamerRaw[] {
    return MapRepository.getMapById(mapId).tamers ?? [];
  }

  public static getIdsByMapId(mapId: string): string[] {
    return this.getByMapId(mapId).map((mapTamerRaw) => {
      return mapTamerRaw.id;
    });
  }
}
