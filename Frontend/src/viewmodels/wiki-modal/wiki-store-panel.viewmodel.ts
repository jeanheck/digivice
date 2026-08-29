import type { WikiStoreCardViewModel } from "@/viewmodels/wiki-modal/wiki-store-card.viewmodel";

export interface WikiStorePanelViewModel {
  cards: WikiStoreCardViewModel[];
  locationId: string | null;
}
