import { MathHelper } from "@/presenters/helper/math.helper";

export class MpProgressBarPresenter {
  public static getCalculatedProgressPercentage(currentMp: number, maxMp: number): number {
    return MathHelper.calculatePercentage(currentMp, maxMp);
  }
}
