export interface WeaponShopInventoryItemRaw {
  equipmentId: string;
  price: number;
}

export interface WeaponShopRaw {
  locationId: string;
  inventory: WeaponShopInventoryItemRaw[];
}
