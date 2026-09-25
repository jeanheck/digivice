import { DigievolutionSelector } from "@/stores/selectors/digievolution.selector";

export class DigievolutionDvxpPresenter {
  public static readonly MAX_DVXP_BY_LEVEL = DigievolutionSelector.DVXP_PER_LEVEL;

  public static getCalculatedDvxp(dvxp: number): number {
    return DigievolutionSelector.getDvxpProgress(dvxp);
  }
}
