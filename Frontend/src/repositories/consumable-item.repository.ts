import ConsumableItemJson from "@/database/consumable-item/consumable-item.json";
import type { ConsumableItemTable } from "@/repositories/tables/consumable-item/consumable-item.table";
import type { ConsumableItemRaw } from "@/repositories/tables/raws/consumable-item/consumable-item.raw";

export class ConsumableItemRepository {
  private static readonly consumableItemTable = ConsumableItemJson as ConsumableItemTable;

  public static getById(consumableItemId: number): ConsumableItemRaw | undefined {
    return this.consumableItemTable[String(consumableItemId)];
  }
}
