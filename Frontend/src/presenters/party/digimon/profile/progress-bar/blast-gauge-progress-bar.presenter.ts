import { MathHelper } from "@/presenters/helper/math.helper";

export class BlastGaugeProgressBarPresenter {
  public static readonly MAX_BLAST_GAUGE = 1000;

  public static calculateProgressPercentage(blastGauge: number): number {
    return MathHelper.calculatePercentage(blastGauge, this.MAX_BLAST_GAUGE);
  }
}
