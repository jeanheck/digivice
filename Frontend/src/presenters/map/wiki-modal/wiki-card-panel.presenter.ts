import { CardConverter } from "@/presenters/converter/card.converter";
import { BoosterRepository } from "@/repositories/booster.repository";
import { CardRepository } from "@/repositories/card.repository";
import { CardShopRepository } from "@/repositories/card-shop.repository";
import type { WikiCardPanelViewModel } from "@/viewmodels/wiki-modal/wiki-card-panel.viewmodel";

export class WikiCardPanelPresenter {
  public static getViewModel(cardId: string): WikiCardPanelViewModel | null {
    const cardRaw = CardRepository.getCardById(cardId);
    if (cardRaw === undefined) {
      return null;
    }

    return {
      card: CardConverter.convert(cardId, cardRaw),
      boosters: BoosterRepository.getBoosterIdsByCardId(Number(cardId)),
      cardShops: CardShopRepository.getCardShopsByCardId(cardId).map((cardShopCardRaw) => ({
        id: cardShopCardRaw.cardShopId,
        mainQuestAvailabilityWindow: cardShopCardRaw.mainQuestAvailabilityWindow,
      })),
    };
  }
}
