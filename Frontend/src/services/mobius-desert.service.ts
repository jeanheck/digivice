import { LocationRegionConstant } from "@/constants/location-region.constant";
import { MobiusDesertAreasMapRepository } from "@/repositories/mobius-desert-areas-map.repository";
import { LocationService } from "@/services/location.service";
import type { DesertAreaMapCellViewModel } from "@/viewmodels/desert/desert-area-map-cell.viewmodel";

export class MobiusDesertService {
  public static isMobiusDesertLocation(locationId: string): boolean {
    return (
      LocationService.getRegionByLocationId(locationId) === LocationRegionConstant.mobiusDesert
    );
  }

  public static getMobiusDesertArea(
    locationId: string,
    mapVariant: number | null,
  ): DesertAreaMapCellViewModel | null {
    if (mapVariant === null) {
      return null;
    }

    const cellRaw = MobiusDesertAreasMapRepository.getMobiusDesertArea(
      locationId,
      String(mapVariant),
    );

    if (cellRaw === null) {
      return null;
    }

    return cellRaw as DesertAreaMapCellViewModel;
  }
}
