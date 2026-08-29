import type { WikiStoreCardViewModel } from "@/viewmodels/wiki-modal/wiki-store-card.viewmodel";

export class WikiStoreCardConverter {
  public static convert(cardId: string, price: number): WikiStoreCardViewModel {
    return {
      cardId,
      nameKey: `cards.${cardId}.name`,
      price,
    };
  }
}
