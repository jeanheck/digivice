import type { WikiEquipmentShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-equipment-shop-item.viewmodel";

export interface WikiEquipmentShopViewModel {
  items: WikiEquipmentShopItemViewModel[];
  locationId: string | null;
}
