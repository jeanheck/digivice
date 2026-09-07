import type { WikiCardBoosterViewModel } from "@/viewmodels/wiki-modal/wiki-card-booster.viewmodel";
import type { WikiCardDetailsViewModel } from "@/viewmodels/wiki-modal/wiki-card-details.viewmodel";
import type { WikiCardStoreViewModel } from "@/viewmodels/wiki-modal/wiki-card-store.viewmodel";

export interface WikiCardPanelViewModel {
  card: WikiCardDetailsViewModel | null;
  boosters: WikiCardBoosterViewModel[];
  stores: WikiCardStoreViewModel[];
}
