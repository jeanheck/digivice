import { WikiCardBoosterConverter } from "@/presenters/converter/wiki-card-booster.converter";
import { WikiCardDetailsConverter } from "@/presenters/converter/wiki-card-details.converter";
import { WikiCardStoreConverter } from "@/presenters/converter/wiki-card-store.converter";
import { MainQuestRangeHelper } from "@/presenters/helper/main-quest-range.helper";
import type { Quest } from "@/models";
import { CardRepository } from "@/repositories/card.repository";
import { DropRepository } from "@/repositories/drop.repository";
import type { CardStoreRaw } from "@/repositories/tables/raws/tcg/card.raw";
import { QuestService } from "@/services/quest.service";
import type { CardBoosterSourceViewModel } from "@/viewmodels/card/card-booster-source.viewmodel";
import type { WikiCardStoreViewModel } from "@/viewmodels/wiki-modal/wiki-card-store.viewmodel";
import type { WikiCardsPanelViewModel } from "@/viewmodels/wiki-modal/wiki-cards-panel.viewmodel";

export class WikiCardsPanelPresenter {
  public static getViewModel(cardId: string, mainQuest: Quest | null): WikiCardsPanelViewModel {
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
      stores: WikiCardsPanelPresenter.getCardStores(cardRaw.stores, mainQuest),
    };
  }

  private static getCardStores(
    stores: CardStoreRaw[] | undefined,
    mainQuest: Quest | null,
  ): WikiCardStoreViewModel[] {
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const activeStoreIds = new Set<string>();
    const result: WikiCardStoreViewModel[] = [];

    for (const store of stores ?? []) {
      if (activeStoreIds.has(store.storeId)) {
        continue;
      }

      const isInRange = MainQuestRangeHelper.isInMainQuestRange(
        store.startWhenLastMainQuestStepDone,
        store.finishWhenLastMainQuestStepDone,
        lastCompletedMainQuestStep,
      );
      if (!isInRange) {
        continue;
      }

      const storeViewModel = WikiCardStoreConverter.convert(store.storeId);
      if (storeViewModel === null) {
        continue;
      }

      activeStoreIds.add(store.storeId);
      result.push(storeViewModel);
    }

    return result;
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
