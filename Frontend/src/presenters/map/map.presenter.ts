import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { LocationRegionConstant } from "@/constants/location-region.constant";
import type { Quest } from "@/models";
import { LocationConverter } from "@/presenters/converter/location.converter";
import { LocationRepository } from "@/repositories/location.repository";
import { LocationService } from "@/services/location.service";
import { MobiusDesertService } from "@/services/mobius-desert.service";
import type { DesertAreaMapCellViewModel } from "@/viewmodels/desert/desert-area-map-cell.viewmodel";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class MapPresenter {
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

  public static getRegionByLocationId(id: string | null): LocationRegionConstant {
    return LocationService.getRegionByLocationId(id);
  }

  public static getLocationImageUrlByLocationId(id: string | null): string | null {
    return ImageCatalog.getLocationImageUrl(LocationService.getLocationImageNameByLocationId(id));
  }

  public static getCell(locationId: string, mapVariant: number): DesertAreaMapCellViewModel | null {
    return MobiusDesertService.getCell(locationId, mapVariant);
  }
}
