import { DigievolutionSelector } from "@/stores/selectors/digievolution.selector";

export class DigievolutionDvexpPresenter {
  public static getCalculatedDvexp(dvexp: number): number {
    return DigievolutionSelector.getDvexpProgress(dvexp);
  }
}
