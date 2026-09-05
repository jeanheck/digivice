const STAT_CAP = 999;

export class StatHelper {
  public static calculateStat(base: number, equipBonus: number): number {
    return Math.min(STAT_CAP, base + equipBonus);
  }
}
