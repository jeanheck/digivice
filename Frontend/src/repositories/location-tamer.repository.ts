import { LocationRepository } from "@/repositories/location.repository";
import type { LocationTamerRaw } from "@/repositories/tables/raws/location/location-tamer.raw";

export class LocationTamerRepository {
  public static getByLocationId(locationId: string): LocationTamerRaw[] {
    return LocationRepository.getLocationById(locationId).tamers ?? [];
  }

  public static getIdsByLocationId(locationId: string): string[] {
    return this.getByLocationId(locationId).map((locationTamerRaw) => {
      return locationTamerRaw.id;
    });
  }
}
