import { CardRepository } from "@/repositories/card.repository";
import type { WikiCardShopInventoryCardViewModel } from "@/viewmodels/wiki-modal/wiki-card-shop-inventory-card.viewmodel";

export class WikiCardShopInventoryCardConverter {
  public static convert(cardId: string, price: number): WikiCardShopInventoryCardViewModel {
    const cardRaw = CardRepository.getCardById(cardId);

    return {
      cardId,
      imageName: cardRaw?.imageName ?? "",
      nameKey: `cards.${cardId}.name`,
      price,
    };
  }
}
