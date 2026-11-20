import { CardRepository } from "@/presentation/repositories/card.repository";
import type { WikiDropBoosterCardViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-drop-booster-card.viewmodel";

export class WikiDropBoosterCardConverter {
  public static convert(cardId: number): WikiDropBoosterCardViewModel {
    const cardIdString = String(cardId);
    const cardRaw = CardRepository.getCardById(cardIdString);

    return {
      cardId: cardIdString,
      imageName: cardRaw?.imageName ?? "",
      nameKey: `card.${cardId}.name`,
    };
  }
}
