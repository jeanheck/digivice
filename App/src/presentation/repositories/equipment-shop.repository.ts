import EquipmentShopJson from "@/data/game/shop/equipment-shop.json";
import type { EquipmentShopTable } from "@/presentation/repositories/tables/shop/equipment-shop.table";
import type { EquipmentShopRaw } from "@/presentation/repositories/tables/raws/shop/equipment-shop.raw";

export class EquipmentShopRepository {
  private static readonly equipmentShopTable = EquipmentShopJson as EquipmentShopTable;
  private static readonly equipmentShopIdsByEquipmentId = this.buildEquipmentShopIdsByEquipmentId();

  private static buildEquipmentShopIdsByEquipmentId(): Map<string, string[]> {
    const equipmentShopIdsByEquipmentId = new Map<string, string[]>();
    for (const [equipmentShopId, equipmentShopRaw] of Object.entries(this.equipmentShopTable)) {
      for (const equipmentId of equipmentShopRaw.inventory) {
        const equipmentShopIds = equipmentShopIdsByEquipmentId.get(equipmentId) ?? [];
        equipmentShopIds.push(equipmentShopId);
        equipmentShopIdsByEquipmentId.set(equipmentId, equipmentShopIds);
      }
    }

    return equipmentShopIdsByEquipmentId;
  }

  public static getById(equipmentShopId: string): EquipmentShopRaw | undefined {
    return this.equipmentShopTable[equipmentShopId];
  }

  public static getTable(): EquipmentShopTable {
    return this.equipmentShopTable;
  }

  public static getIds(): string[] {
    return Object.keys(this.equipmentShopTable);
  }

  public static getEquipmentShopIdsByEquipmentId(equipmentId: string): string[] {
    return this.equipmentShopIdsByEquipmentId.get(equipmentId) ?? [];
  }
}
