import { CardConverter } from "@/presenters/converter/card.converter";
import { CardRepository } from "@/repositories/card.repository";
import type { CardViewModel } from "@/viewmodels/card/card.viewmodel";

export class WikiCardPanelPresenter {
  public static getCard(cardId: string): CardViewModel | null {
    const cardRaw = CardRepository.getCardById(cardId);
    if (cardRaw === undefined) {
      return null;
    }

    return CardConverter.convert(cardId, cardRaw);
  }
}
