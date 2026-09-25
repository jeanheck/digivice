import { LocationRepository } from "@/repositories/location.repository";
import type { LocationBossRaw } from "@/repositories/tables/raws/location/location-boss.raw";

export class LocationBossRepository {
  public static getByLocationId(locationId: string): LocationBossRaw[] {
    return LocationRepository.getLocationById(locationId).enemies?.boss ?? [];
  }

  public static getIdsByLocationId(locationId: string): string[] {
    return this.getByLocationId(locationId).map((locationBossRaw) => {
      return locationBossRaw.id;
    });
  }
}
