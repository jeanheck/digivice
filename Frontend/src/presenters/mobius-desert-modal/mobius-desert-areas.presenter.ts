import { DesertAreasRepository } from "@/repositories/desert-areas.repository";
import type { DesertAreasViewModel } from "@/viewmodels/desert/desert-areas.viewmodel";

export class MobiusDesertAreasPresenter {
  public static getAreas(): DesertAreasViewModel {
    return DesertAreasRepository.getAll() as DesertAreasViewModel;
  }
}
