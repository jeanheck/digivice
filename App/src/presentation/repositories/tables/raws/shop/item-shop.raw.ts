import type { EquipmentShopInventoryOrder } from "@/presentation/repositories/tables/raws/shop/equipment-shop.raw";

export interface ItemShopRaw {
  locationId: string;
  imageName?: string;
  inventoryOrder: EquipmentShopInventoryOrder;
  inventory: string[];
}
