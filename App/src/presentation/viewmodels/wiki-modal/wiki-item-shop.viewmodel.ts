import type { WikiItemShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-item-shop-item.viewmodel";

export interface WikiItemShopViewModel {
  items: WikiItemShopItemViewModel[];
  locationId: string | null;
}
