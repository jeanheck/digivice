import { DesertAreasRepository } from "@/repositories/desert-areas.repository";
import type { DesertAreasViewModel } from "@/viewmodels/desert/desert-areas.viewmodel";

export class DesertPresenter {
  public static getAreas(): DesertAreasViewModel {
    return DesertAreasRepository.getAll() as DesertAreasViewModel;
  }
}
