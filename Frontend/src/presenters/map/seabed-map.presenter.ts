import { LocationConverter } from "@/presenters/converter/location.converter";
import { LocationRepository } from "@/repositories/location.repository";
import { LocationService } from "@/services/location.service";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class SeabedMapPresenter {
  public static getLocation(locationId: string, seabedRoute: number = 0): LocationViewModel {
    const locationRaw = LocationRepository.getLocationById(locationId);
    const enemyIds = LocationService.getSeabedEnemies(seabedRoute);

    return LocationConverter.convert(locationId, locationRaw, enemyIds);
  }
}
