import type { WikiCardShopInventoryCardViewModel } from "@/viewmodels/wiki-modal/wiki-card-shop-inventory-card.viewmodel";

export interface WikiCardShopPanelViewModel {
  cards: WikiCardShopInventoryCardViewModel[];
  locationId: string | null;
}
