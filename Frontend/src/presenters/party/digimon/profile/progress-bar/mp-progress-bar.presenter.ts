export class MpProgressBarPresenter {
  public static getCalculatedProgressPercentage(currentMp: number, maxMp: number): number {
    return Math.calculatePercentage(currentMp, maxMp);
  }
}
