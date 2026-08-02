import { MathHelper } from "@/presenters/helper/math.helper";

export class BlastProgressBarPresenter {
  public static readonly MAX_BLAST = 1000;

  public static calculateProgressPercentage(blast: number): number {
    return MathHelper.calculatePercentage(blast, this.MAX_BLAST);
  }

  public static getFillEffectClass(progressPercentage: number): string {
    if (progressPercentage >= 80) {
      return "blast-fill-high";
    }

    if (progressPercentage >= 50) {
      return "blast-fill-mid";
    }

    return "";
  }

  public static getTrackEffectClass(progressPercentage: number): string {
    if (progressPercentage >= 80) {
      return "blast-track-high";
    }

    if (progressPercentage >= 50) {
      return "blast-track-mid";
    }

    return "";
  }
}
