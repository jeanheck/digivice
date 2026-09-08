import type { CardViewModel } from "@/viewmodels/card/card.viewmodel";
import type { CardShopSourceViewModel } from "@/viewmodels/card/card-shop-source.viewmodel";

export interface WikiCardPanelViewModel {
  card: CardViewModel;
  boosters: number[];
  cardShops: CardShopSourceViewModel[];
}
