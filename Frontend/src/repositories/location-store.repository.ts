import { LocationRepository } from "@/repositories/location.repository";
import type { LocationStoreRaw } from "@/repositories/tables/raws/location/location-store.raw";

export class LocationStoreRepository {
  public static getByLocationId(locationId: string): LocationStoreRaw[] {
    return LocationRepository.getLocationById(locationId).stores ?? [];
  }

  public static getIdsByLocationId(locationId: string): string[] {
    return this.getByLocationId(locationId).map((locationStoreRaw) => {
      return locationStoreRaw.id;
    });
  }
}
