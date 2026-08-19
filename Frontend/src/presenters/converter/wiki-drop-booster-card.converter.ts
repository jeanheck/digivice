import type { WikiDropBoosterCardViewModel } from "@/viewmodels/wiki-modal/wiki-drop-booster-card.viewmodel";

export class WikiDropBoosterCardConverter {
  public static convert(cardId: number): WikiDropBoosterCardViewModel {
    return {
      cardId: String(cardId),
      nameKey: `cards.${cardId}.name`,
    };
  }
}
