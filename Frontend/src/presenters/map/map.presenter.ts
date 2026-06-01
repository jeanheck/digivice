import { LocationConverter } from "@/presenters/converter/location.converter";
import { LocationRepository } from "@/repositories/location.repository";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class MapPresenter {
  public static getLocationById(locationId: string): LocationViewModel {
    const locationRaw = LocationRepository.getLocationById(locationId);

    return LocationConverter.convert(locationId, locationRaw);
  }
}
