import type { WikiCardShopInventoryCardViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-card-shop-inventory-card.viewmodel";

export interface WikiCardShopViewModel {
  cards: WikiCardShopInventoryCardViewModel[];
  locationId: string | null;
}
