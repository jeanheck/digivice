import { WikiCardBoosterConverter } from "@/presenters/converter/wiki-card-booster.converter";
import { WikiCardDetailsConverter } from "@/presenters/converter/wiki-card-details.converter";
import { WikiCardStoreConverter } from "@/presenters/converter/wiki-card-store.converter";
import { CardRepository } from "@/repositories/card.repository";
import { DropRepository } from "@/repositories/drop.repository";
import type { CardBoosterSourceViewModel } from "@/viewmodels/card/card-booster-source.viewmodel";
import type { WikiCardStoreViewModel } from "@/viewmodels/wiki-modal/wiki-card-store.viewmodel";
import type { WikiCardsPanelViewModel } from "@/viewmodels/wiki-modal/wiki-cards-panel.viewmodel";

export class WikiCardsPanelPresenter {
  public static getViewModel(cardId: string): WikiCardsPanelViewModel {
    const cardRaw = CardRepository.getCardById(cardId);
    if (cardRaw === undefined) {
      return {
        card: null,
        sources: [],
        stores: [],
      };
    }

    return {
      card: WikiCardDetailsConverter.convert(cardId, cardRaw),
      sources: WikiCardsPanelPresenter.getCardBoosterSources(cardRaw.boosters).map((source) => {
        return WikiCardBoosterConverter.convert(source);
      }),
      stores: WikiCardsPanelPresenter.getCardStores(cardRaw.stores ?? []),
    };
  }

  private static getCardStores(storeIds: string[]): WikiCardStoreViewModel[] {
    const stores: WikiCardStoreViewModel[] = [];

    for (const storeId of storeIds) {
      const store = WikiCardStoreConverter.convert(storeId);
      if (store !== null) {
        stores.push(store);
      }
    }

    return stores;
  }

  private static getCardBoosterSources(boosterIds: number[]): CardBoosterSourceViewModel[] {
    const sources: CardBoosterSourceViewModel[] = [];

    for (const boosterId of boosterIds) {
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
