export type EquipmentShopInventoryOrder = "fixed" | "alphabetical";

export interface EquipmentShopRaw {
  locationId: string;
  imageName?: string;
  inventoryOrder: EquipmentShopInventoryOrder;
  inventory: string[];
}
