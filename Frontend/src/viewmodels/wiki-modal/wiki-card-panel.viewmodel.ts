import type { CardViewModel } from "@/viewmodels/card/card.viewmodel";
import type { CardShopViewModel } from "@/viewmodels/card/card-shop.viewmodel";

export interface WikiCardPanelViewModel {
  card: CardViewModel;
  boosters: number[];
  cardShops: CardShopViewModel[];
}
