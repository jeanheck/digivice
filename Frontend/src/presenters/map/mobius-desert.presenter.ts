import { DesertAreasMapRepository } from "@/repositories/desert-areas-map.repository";
import type { DesertAreaDetailsViewModel } from "@/viewmodels/desert/desert-area-details.viewmodel";
import type { DesertAreaMapCellViewModel } from "@/viewmodels/desert/desert-area-map-cell.viewmodel";

export class MobiusDesertPresenter {
  public static getAreaDetails(label: string): DesertAreaDetailsViewModel | null {
    const areaRaw = DesertAreasMapRepository.findByLabel(label);

    if (areaRaw === null) {
      return null;
    }

    return {
      locationId: areaRaw.locationId,
      coordinates: areaRaw.cell.coordinates ?? null
    };
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
