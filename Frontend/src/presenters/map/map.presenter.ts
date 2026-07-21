import type { Quest } from "@/models";
import { LocationConverter } from "@/presenters/converter/location.converter";
import { DesertAreasMapRepository } from "@/repositories/desert-areas-map.repository";
import { LocationRepository } from "@/repositories/location.repository";
import { LocationService } from "@/services/location.service";
import type { DesertAreaMapCellViewModel } from "@/viewmodels/desert/desert-area-map-cell.viewmodel";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class MapPresenter {
  private static readonly mobiusDesertLocationIds: ReadonlySet<string> = new Set(["0258", "0259"]);

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

  public static isSeabedLocation(locationId: string | null): boolean {
    return LocationService.isSeabed(locationId);
  }

  public static isMobiusDesertLocation(locationId: string | null): boolean {
    if (locationId === null) {
      return false;
    }

    return MapPresenter.mobiusDesertLocationIds.has(locationId);
  }

  public static getCell(locationId: string, mapVariant: number): DesertAreaMapCellViewModel | null {
    if (mapVariant <= 0) {
      return null;
    }

    const cellRaw = DesertAreasMapRepository.getCell(locationId, String(mapVariant));

    if (cellRaw === null) {
      return null;
    }

    return cellRaw as DesertAreaMapCellViewModel;
  }
}
