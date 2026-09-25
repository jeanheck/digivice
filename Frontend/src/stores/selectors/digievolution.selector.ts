export class DigievolutionSelector {
  public static readonly DVEXP_PER_LEVEL = 10;

  public static getDvexpProgress(dvexp: number): number {
    return dvexp % this.DVEXP_PER_LEVEL;
  }
}
