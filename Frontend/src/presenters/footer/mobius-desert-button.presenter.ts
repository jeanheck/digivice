import type { Quest } from "@/models";
import { LocationRepository } from "@/repositories";
import { LocationService } from "@/services/location.service";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";
import { LocationConverter } from "../converter/location.converter";

export class MobiusDesertButtonPresenter {
  public static getLocation(
    locationId: string,
    mainQuest: Quest | null,
    seabedRoute: number = 0,
  ): LocationViewModel {
    const locationRaw = LocationRepository.getLocationById(locationId);
    const enemyIds = LocationService.getCurrentEnemies(locationId, mainQuest, seabedRoute);

    return LocationConverter.convert(locationId, locationRaw, enemyIds);
  }
}
