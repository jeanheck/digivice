import { CardConverter } from "@/presenters/converter/card.converter";
import { CardRepository } from "@/repositories/card.repository";
import type { WikiCardPanelViewModel } from "@/viewmodels/wiki-modal/wiki-card-panel.viewmodel";

export class WikiCardPanelPresenter {
  public static getViewModel(cardId: string): WikiCardPanelViewModel | null {
    const cardRaw = CardRepository.getCardById(cardId);
    if (cardRaw === undefined) {
      return null;
    }

    return {
      card: CardConverter.convert(cardId, cardRaw),
      boosters: cardRaw.boosters,
      cardShops: (cardRaw.stores ?? []).map((store) => ({
        storeId: store.storeId,
        startWhenLastMainQuestStepDone: store.startWhenLastMainQuestStepDone,
        finishWhenLastMainQuestStepDone: store.finishWhenLastMainQuestStepDone,
      })),
    };
  }
}
