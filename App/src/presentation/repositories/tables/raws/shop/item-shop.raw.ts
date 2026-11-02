import type { EquipmentShopInventoryOrder } from "@/presentation/repositories/tables/raws/shop/equipment-shop.raw";

export interface ItemShopInventoryItemRaw {
  consumableItemId: string;
  price: number;
}

export interface ItemShopRaw {
  locationId: string;
  imageName?: string;
  inventoryOrder: EquipmentShopInventoryOrder;
  inventory: ItemShopInventoryItemRaw[];
}
