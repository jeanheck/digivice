import type { Quest } from "@/models";
import { LocationConverter } from "@/presenters/converter/location.converter";
import { LocationRepository } from "@/repositories/location.repository";
import { LocationService } from "@/services/location.service";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class SeabedMapPresenter {
  public static getLocation(
    locationId: string,
    mainQuest: Quest | null,
    seabedRoute: number = 0,
    previousMapId: string = "",
  ): LocationViewModel {
    const locationRaw = LocationRepository.getLocationById(locationId);
    const enemyIds = LocationService.getCurrentEnemies(locationId, mainQuest, seabedRoute, previousMapId);

    return LocationConverter.convert(locationId, locationRaw, enemyIds);
  }
}
