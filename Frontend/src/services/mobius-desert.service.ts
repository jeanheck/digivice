import { LocationRegionConstant } from "@/constants/location-region.constant";
import { MobiusDesertAreasMapRepository } from "@/repositories/mobius-desert-areas-map.repository";
import { LocationService } from "@/services/location.service";
import type { DesertAreaMapCellViewModel } from "@/viewmodels/desert/desert-area-map-cell.viewmodel";

export class MobiusDesertService {
  public static isMobiusDesertLocation(locationId: string | null): boolean {
    return LocationService.getRegionByLocationId(locationId) === LocationRegionConstant.mobiusDesert;
  }

  public static getCell(locationId: string, mapVariant: number): DesertAreaMapCellViewModel | null {
    if (mapVariant <= 0) {
      return null;
    }

    const cellRaw = MobiusDesertAreasMapRepository.getCell(locationId, String(mapVariant));

    if (cellRaw === null) {
      return null;
    }

    return cellRaw as DesertAreaMapCellViewModel;
  }
}
