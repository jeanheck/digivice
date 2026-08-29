export class StatCapHelper {
  public static readonly cap = 999;

  public static capBasePlusEquip(base: number, equipBonus: number): number {
    return Math.min(this.cap, base + equipBonus);
  }
}
