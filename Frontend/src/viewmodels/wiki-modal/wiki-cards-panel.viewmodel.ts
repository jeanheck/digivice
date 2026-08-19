import type { WikiCardBoosterViewModel } from "@/viewmodels/wiki-modal/wiki-card-booster.viewmodel";
import type { WikiCardDetailsViewModel } from "@/viewmodels/wiki-modal/wiki-card-details.viewmodel";

export interface WikiCardsPanelViewModel {
  card: WikiCardDetailsViewModel | null;
  sources: WikiCardBoosterViewModel[];
}
