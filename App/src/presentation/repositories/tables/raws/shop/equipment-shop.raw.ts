export type EquipmentShopInventoryOrder = "fixed" | "alphabetical";

export interface EquipmentShopInventoryItemRaw {
  equipmentId: string;
  price: number;
}

export interface EquipmentShopRaw {
  locationId: string;
  imageName?: string;
  inventoryOrder: EquipmentShopInventoryOrder;
  inventory: EquipmentShopInventoryItemRaw[];
}
