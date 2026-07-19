import { LocationConverter } from "@/presenters/converter/location.converter";
import { LocationRepository } from "@/repositories/location.repository";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class MapPresenter {
  private static readonly mobiusDesertLocationIds: ReadonlySet<string> = new Set(["0258", "0259"]);

  public static getLocationById(locationId: string, enemyIds: string[]): LocationViewModel {
    const locationRaw = LocationRepository.getLocationById(locationId);

    return LocationConverter.convert(locationId, locationRaw, enemyIds);
  }

  public static isMobiusDesertLocation(locationId: string | null): boolean {
    if (locationId === null) {
      return false;
    }

    return MapPresenter.mobiusDesertLocationIds.has(locationId);
  }
}
