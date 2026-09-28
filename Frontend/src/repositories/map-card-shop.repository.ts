import { MapRepository } from "@/repositories/map.repository";
import type { MapCardShopRaw } from "@/repositories/tables/raws/map/map-card-shop.raw";

export class MapCardShopRepository {
  public static getByMapId(mapId: string): MapCardShopRaw[] {
    return MapRepository.getMapById(mapId).cardShops ?? [];
  }

  public static getIdsByMapId(mapId: string): string[] {
    return this.getByMapId(mapId).map((mapCardShopRaw) => {
      return mapCardShopRaw.id;
    });
  }
}
