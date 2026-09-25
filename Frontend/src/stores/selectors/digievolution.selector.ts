export class DigievolutionSelector {
  public static readonly DVXP_PER_LEVEL = 10;

  public static getDvxpProgress(dvxp: number): number {
    return dvxp % this.DVXP_PER_LEVEL;
  }
}
