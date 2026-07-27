import { MathHelper } from "@/presenters/helper/math.helper";

export class BlastGaugeProgressBarPresenter {
  public static readonly MAX_BLAST_GAUGE = 1000;

  public static calculateProgressPercentage(blastGauge: number): number {
    return MathHelper.calculatePercentage(blastGauge, this.MAX_BLAST_GAUGE);
  }

  public static getFillEffectClass(progressPercentage: number): string {
    if (progressPercentage >= 80) {
      return "blast-gauge-fill-high";
    }

    if (progressPercentage >= 50) {
      return "blast-gauge-fill-mid";
    }

    return "";
  }

  public static getTrackEffectClass(progressPercentage: number): string {
    if (progressPercentage >= 80) {
      return "blast-gauge-track-high";
    }

    if (progressPercentage >= 50) {
      return "blast-gauge-track-mid";
    }

    return "";
  }
}
