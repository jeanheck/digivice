import { LocationRepository } from "@/repositories/location.repository";
import type { LocationDuelIslandRaw } from "@/repositories/tables/raws/location/location-duel-island.raw";

export class LocationDuelIslandRepository {
  public static getByLocationId(locationId: string): LocationDuelIslandRaw[] {
    return LocationRepository.getLocationById(locationId).duelIsland ?? [];
  }

  public static getIdsByLocationId(locationId: string): string[] {
    return this.getByLocationId(locationId).map((locationDuelIslandRaw) => {
      return locationDuelIslandRaw.id;
    });
  }
}
