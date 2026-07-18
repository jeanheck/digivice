import { MathHelper } from "@/presenters/helper/math.helper";

export class HpProgressBarPresenter {
  public static getCalculatedProgressPercentage(currentHp: number, maxHp: number): number {
    return MathHelper.calculatePercentage(currentHp, maxHp);
  }
}
