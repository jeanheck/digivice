const StatCap = 999;

export class StatService {
  public static calculateStat(base: number, equipBonus: number): number {
    return Math.min(StatCap, base + equipBonus);
  }
}
