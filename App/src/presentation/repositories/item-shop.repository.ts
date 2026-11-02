import ItemShopJson from "@/data/game/shop/item-shop.json";
import type { ItemShopTable } from "@/presentation/repositories/tables/shop/item-shop.table";
import type { ItemShopRaw } from "@/presentation/repositories/tables/raws/shop/item-shop.raw";

export class ItemShopRepository {
  private static readonly itemShopTable = ItemShopJson as ItemShopTable;
  private static readonly itemShopIdsByConsumableItemId = this.buildItemShopIdsByConsumableItemId();

  private static buildItemShopIdsByConsumableItemId(): Map<string, string[]> {
    const itemShopIdsByConsumableItemId = new Map<string, string[]>();
    for (const [itemShopId, itemShopRaw] of Object.entries(this.itemShopTable)) {
      for (const inventoryItem of itemShopRaw.inventory) {
        const itemShopIds = itemShopIdsByConsumableItemId.get(inventoryItem.consumableItemId) ?? [];
        itemShopIds.push(itemShopId);
        itemShopIdsByConsumableItemId.set(inventoryItem.consumableItemId, itemShopIds);
      }
    }

    return itemShopIdsByConsumableItemId;
  }

  public static getById(itemShopId: string): ItemShopRaw | undefined {
    return this.itemShopTable[itemShopId];
  }

  public static getTable(): ItemShopTable {
    return this.itemShopTable;
  }

  public static getIds(): string[] {
    return Object.keys(this.itemShopTable);
  }

  public static getItemShopIdsByConsumableItemId(consumableItemId: string): string[] {
    return this.itemShopIdsByConsumableItemId.get(consumableItemId) ?? [];
  }
}
