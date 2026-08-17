import { WikiCardBoosterConverter } from "@/presenters/converter/wiki-card-booster.converter";
import { CardRepository } from "@/repositories/card.repository";
import { DropRepository } from "@/repositories/drop.repository";
import type { CardBoosterSourceViewModel } from "@/viewmodels/card/card-booster-source.viewmodel";
import type { WikiCardsPanelViewModel } from "@/viewmodels/wiki-modal/wiki-cards-panel.viewmodel";

export class WikiCardsPanelPresenter {
  public static getViewModel(cardId: string): WikiCardsPanelViewModel {
    return {
      noteKey: `cards.${cardId}.note`,
      sources: WikiCardsPanelPresenter.getCardBoosterSources(cardId).map((source) => {
        return WikiCardBoosterConverter.convert(source);
      }),
    };
  }

  private static getCardBoosterSources(cardId: string): CardBoosterSourceViewModel[] {
    const cardRaw = CardRepository.getCardById(cardId);
    if (cardRaw === undefined) {
      return [];
    }

    const sources: CardBoosterSourceViewModel[] = [];

    for (const boosterId of cardRaw.boosters) {
      const dropKey = DropRepository.getDropKeyByNumericId(boosterId);
      if (dropKey === undefined) {
        continue;
      }

      sources.push({
        dropKey,
        boosterId,
      });
    }

    return sources;
  }
}
