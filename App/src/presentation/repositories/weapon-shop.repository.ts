import WeaponShopJson from "@/data/game/shop/weapon-shop.json";
import type { WeaponShopTable } from "@/presentation/repositories/tables/shop/weapon-shop.table";
import type { WeaponShopRaw } from "@/presentation/repositories/tables/raws/shop/weapon-shop.raw";

export class WeaponShopRepository {
  private static readonly weaponShopTable = WeaponShopJson as WeaponShopTable;

  public static getById(weaponShopId: string): WeaponShopRaw | undefined {
    return this.weaponShopTable[weaponShopId];
  }

  public static getTable(): WeaponShopTable {
    return this.weaponShopTable;
  }

  public static getIds(): string[] {
    return Object.keys(this.weaponShopTable);
  }
}
