import { MobiusDesertAreasMapRepository } from "@/repositories/mobius-desert-areas-map.repository";
import type { DesertAreaMapCellViewModel } from "@/viewmodels/desert/desert-area-map-cell.viewmodel";

export class MobiusDesertService {
  private static readonly mobiusDesertLocationIds: ReadonlySet<string> = new Set(["0258", "0259"]);

  public static isMobiusDesertLocation(locationId: string | null): boolean {
    if (locationId === null) {
      return false;
    }

    return MobiusDesertService.mobiusDesertLocationIds.has(locationId);
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
