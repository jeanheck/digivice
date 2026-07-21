import type { Quest } from "@/models";
import { LocationConverter } from "@/presenters/converter/location.converter";
import { LocationRepository } from "@/repositories/location.repository";
import { LocationService } from "@/services/location.service";
import { MobiusDesertService } from "@/services/mobius-desert.service";
import type { DesertAreaMapCellViewModel } from "@/viewmodels/desert/desert-area-map-cell.viewmodel";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class MobiusDesertMapPresenter {
  public static getLocation(
    locationId: string,
    mainQuest: Quest | null,
    seabedRoute: number = 0,
  ): LocationViewModel {
    const locationRaw = LocationRepository.getLocationById(locationId);
    const enemyIds = LocationService.getCurrentEnemies(locationId, mainQuest, seabedRoute);

    return LocationConverter.convert(locationId, locationRaw, enemyIds);
  }

  public static getCell(locationId: string, mapVariant: number): DesertAreaMapCellViewModel | null {
    return MobiusDesertService.getCell(locationId, mapVariant);
  }
}
