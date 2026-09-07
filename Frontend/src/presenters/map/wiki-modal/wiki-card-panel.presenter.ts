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
import type { CardShopViewModel } from "@/viewmodels/card/card-shop.viewmodel";
import type { WikiCardPanelViewModel } from "@/viewmodels/wiki-modal/wiki-card-panel.viewmodel";

export class WikiCardPanelPresenter {
  public static getViewModel(cardId: string, mainQuest: Quest | null): WikiCardPanelViewModel {
    const cardRaw = CardRepository.getCardById(cardId);
    if (cardRaw === undefined) {
      return {
        card: null,
        boosters: [],
        stores: [],
      };
    }

    return {
      card: WikiCardDetailsConverter.convert(cardId, cardRaw),
      boosters: WikiCardPanelPresenter.getCardBoosters(cardRaw.boosters).map((booster) => {
        return WikiCardBoosterConverter.convert(booster);
      }),
      stores: WikiCardPanelPresenter.getCardStores(cardRaw.stores, mainQuest),
    };
  }

  private static getCardStores(
    stores: CardStoreRaw[] | undefined,
    mainQuest: Quest | null,
  ): CardShopViewModel[] {
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const activeStoreIds = new Set<string>();
    const result: CardShopViewModel[] = [];

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

  private static getCardBoosters(boosterIds: number[]): CardBoosterSourceViewModel[] {
    const boosters: CardBoosterSourceViewModel[] = [];

    for (const boosterId of boosterIds) {
      const dropKey = DropRepository.getDropKeyByNumericId(boosterId);
      if (dropKey === undefined) {
        continue;
      }

      boosters.push({
        dropKey,
        boosterId,
      });
    }

    return boosters;
  }
}
