import { LocationRepository } from "@/repositories/location.repository";
import type { LocationCardShopRaw } from "@/repositories/tables/raws/location/location-card-shop.raw";

export class LocationCardShopRepository {
  public static getByLocationId(locationId: string): LocationCardShopRaw[] {
    return LocationRepository.getLocationById(locationId).cardShops ?? [];
  }

  public static getIdsByLocationId(locationId: string): string[] {
    return this.getByLocationId(locationId).map((locationCardShopRaw) => {
      return locationCardShopRaw.id;
    });
  }
}
