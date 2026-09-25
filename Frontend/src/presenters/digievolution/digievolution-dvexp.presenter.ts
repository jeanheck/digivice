import { DigievolutionSelector } from "@/stores/selectors/digievolution.selector";

export class DigievolutionDvexpPresenter {
  public static readonly MAX_DVEXP_BY_LEVEL = DigievolutionSelector.DVEXP_PER_LEVEL;

  public static getCalculatedDvexp(dvexp: number): number {
    return DigievolutionSelector.getDvexpProgress(dvexp);
  }
}
