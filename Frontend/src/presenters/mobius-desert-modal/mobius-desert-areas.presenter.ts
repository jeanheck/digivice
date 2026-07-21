import { MobiusDesertAreasRepository } from "@/repositories/mobius-desert-areas.repository";
import type { DesertAreasViewModel } from "@/viewmodels/desert/desert-areas.viewmodel";

export class MobiusDesertAreasPresenter {
  public static getAreas(): DesertAreasViewModel {
    return MobiusDesertAreasRepository.getAll() as DesertAreasViewModel;
  }
}
