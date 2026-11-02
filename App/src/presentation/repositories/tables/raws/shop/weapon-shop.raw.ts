export interface WeaponShopInventoryItemRaw {
  equipmentId: string;
  price: number;
}

export interface WeaponShopRaw {
  locationId: string;
  imageName?: string;
  inventory: WeaponShopInventoryItemRaw[];
}
