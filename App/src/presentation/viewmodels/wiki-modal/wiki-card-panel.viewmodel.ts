import type { CardViewModel } from "@/presentation/viewmodels/card/card.viewmodel";
import type { CardShopViewModel } from "@/presentation/viewmodels/card/card-shop.viewmodel";

export interface WikiCardPanelViewModel {
  card: CardViewModel;
  boosters: number[];
  cardShops: CardShopViewModel[];
}
