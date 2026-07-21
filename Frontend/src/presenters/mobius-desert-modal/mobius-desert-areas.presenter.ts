import { MobiusDesertAreasRepository } from "@/repositories/mobius-desert-areas.repository";
import { MobiusDesertService } from "@/services/mobius-desert.service";
import type { DesertAreaViewModel } from "@/viewmodels/desert/desert-area.viewmodel";
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

  public static isNoiseDesertFamily(area: DesertAreaViewModel): boolean {
    if (area.type === "noiseDesertS") {
      return true;
    }

    return area.type === "border" && area.label === "noiseDesertS";
  }

  public static shouldDrawRightConnection(
    source: DesertAreaViewModel,
    rightNeighbor: DesertAreaViewModel | null,
  ): boolean {
    if (rightNeighbor === null) {
      return false;
    }

    if (MobiusDesertAreasPresenter.isNoiseDesertFamily(source)) {
      return false;
    }

    if (MobiusDesertAreasPresenter.isNoiseDesertFamily(rightNeighbor)) {
      return false;
    }

    if (source.type === "mirageTower") {
      return rightNeighbor.label === "C1";
    }

    return true;
  }

  public static shouldDrawBottomConnection(
    source: DesertAreaViewModel,
    bottomNeighbor: DesertAreaViewModel | null,
  ): boolean {
    if (bottomNeighbor === null) {
      return false;
    }

    if (source.type === "mirageTower") {
      return false;
    }

    if (bottomNeighbor.type === "mirageTower") {
      return false;
    }

    return true;
  }
}
