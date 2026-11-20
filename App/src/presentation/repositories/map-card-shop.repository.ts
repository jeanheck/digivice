import { MapRepository } from "@/presentation/repositories/map.repository";
import type { MapShopRaw } from "@/presentation/repositories/tables/raws/map/map-shop.raw";

export class MapCardShopRepository {
  public static getByMapId(mapId: string): MapShopRaw[] {
    return MapRepository.getMapById(mapId).cardShops ?? [];
  }

  public static getIdsByMapId(mapId: string): string[] {
    return this.getByMapId(mapId).map((mapCardShopRaw) => {
      return mapCardShopRaw.id;
    });
  }
}
