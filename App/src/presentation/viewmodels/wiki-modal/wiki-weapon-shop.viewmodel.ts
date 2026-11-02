import type { WikiWeaponShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-weapon-shop-item.viewmodel";

export interface WikiWeaponShopViewModel {
  items: WikiWeaponShopItemViewModel[];
  locationId: string | null;
}
