import WeaponShopJson from "@/data/game/shop/weapon-shop.json";
import type { WeaponShopTable } from "@/presentation/repositories/tables/shop/weapon-shop.table";
import type { WeaponShopRaw } from "@/presentation/repositories/tables/raws/shop/weapon-shop.raw";

export class WeaponShopRepository {
  private static readonly weaponShopTable = WeaponShopJson as WeaponShopTable;
  private static readonly weaponShopIdsByEquipmentId = this.buildWeaponShopIdsByEquipmentId();

  private static buildWeaponShopIdsByEquipmentId(): Map<string, string[]> {
    const weaponShopIdsByEquipmentId = new Map<string, string[]>();
    for (const [weaponShopId, weaponShopRaw] of Object.entries(this.weaponShopTable)) {
      for (const inventoryItem of weaponShopRaw.inventory) {
        const weaponShopIds = weaponShopIdsByEquipmentId.get(inventoryItem.equipmentId) ?? [];
        weaponShopIds.push(weaponShopId);
        weaponShopIdsByEquipmentId.set(inventoryItem.equipmentId, weaponShopIds);
      }
    }

    return weaponShopIdsByEquipmentId;
  }

  public static getById(weaponShopId: string): WeaponShopRaw | undefined {
    return this.weaponShopTable[weaponShopId];
  }

  public static getTable(): WeaponShopTable {
    return this.weaponShopTable;
  }

  public static getIds(): string[] {
    return Object.keys(this.weaponShopTable);
  }

  public static getWeaponShopIdsByEquipmentId(equipmentId: string): string[] {
    return this.weaponShopIdsByEquipmentId.get(equipmentId) ?? [];
  }
}
