import { CardRepository } from "@/repositories/card.repository";
import type { WikiStoreCardViewModel } from "@/viewmodels/wiki-modal/wiki-store-card.viewmodel";

export class WikiStoreCardConverter {
  public static convert(cardId: string, price: number): WikiStoreCardViewModel {
    const cardRaw = CardRepository.getCardById(cardId);

    return {
      cardId,
      imageName: cardRaw?.imageName ?? "",
      nameKey: `cards.${cardId}.name`,
      price,
    };
  }
}
