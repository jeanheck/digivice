import { LocationRepository } from "@/repositories";
import { LocationService } from "@/services/location.service";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";
import { LocationConverter } from "../converter/location.converter";

export class SeabedButtonPresenter {
  public static getLocation(locationId: string, seabedRoute: number = 0): LocationViewModel {
    const locationRaw = LocationRepository.getLocationById(locationId);
    const enemyIds = LocationService.getSeabedEnemies(seabedRoute);

    return LocationConverter.convert(locationId, locationRaw, enemyIds);
  }
}
