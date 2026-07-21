import { MobiusDesertAreasRepository } from "@/repositories/mobius-desert-areas.repository";
import { MobiusDesertService } from "@/services/mobius-desert.service";
import type { DesertAreasViewModel } from "@/viewmodels/desert/desert-areas.viewmodel";

export class MobiusDesertAreasPresenter {
  public static getAreas(): DesertAreasViewModel {
    return MobiusDesertAreasRepository.getAll() as DesertAreasViewModel;
  }

  public static getCurrentAreaLabel(locationId: string | null, mapVariant: number): string | null {
    if (locationId === null || !MobiusDesertService.isMobiusDesertLocation(locationId)) {
      return null;
    }

    const cell = MobiusDesertService.getCell(locationId, mapVariant);

    if (cell === null) {
      return null;
    }

    return cell.label;
  }
}
