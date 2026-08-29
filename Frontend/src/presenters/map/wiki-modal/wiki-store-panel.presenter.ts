import { WikiStoreCardConverter } from "@/presenters/converter/wiki-store-card.converter";
import { MainQuestRangeHelper } from "@/presenters/helper/main-quest-range.helper";
import type { Quest } from "@/models";
import { CardRepository } from "@/repositories/card.repository";
import { StoreRepository } from "@/repositories/store.repository";
import type { StoreInventoryItemRaw, StorePhaseRaw } from "@/repositories/tables/raws/tcg/store.raw";
import { QuestService } from "@/services/quest.service";
import type { WikiStoreCardViewModel } from "@/viewmodels/wiki-modal/wiki-store-card.viewmodel";
import type { WikiStorePanelViewModel } from "@/viewmodels/wiki-modal/wiki-store-panel.viewmodel";

export class WikiStorePanelPresenter {
  public static getViewModel(storeId: string, mainQuest: Quest | null): WikiStorePanelViewModel {
    const storeRaw = StoreRepository.getStoreById(storeId);
    if (storeRaw === undefined) {
      return {
        cards: [],
        locationId: null,
      };
    }

    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const inventory = WikiStorePanelPresenter.getActiveInventory(storeRaw.phases, lastCompletedMainQuestStep);

    return {
      cards: WikiStorePanelPresenter.getStoreCards(inventory),
      locationId: storeRaw.locationId,
    };
  }

  private static getActiveInventory(
    phases: StorePhaseRaw[],
    lastCompletedMainQuestStep: number,
  ): StoreInventoryItemRaw[] {
    const inventory: StoreInventoryItemRaw[] = [];

    for (const phase of phases) {
      const isInRange = MainQuestRangeHelper.isInMainQuestRange(
        phase.startWhenLastMainQuestStepDone,
        phase.finishWhenLastMainQuestStepDone,
        lastCompletedMainQuestStep,
      );
      if (!isInRange) {
        continue;
      }

      inventory.push(...phase.inventory);
    }

    return inventory;
  }

  private static getStoreCards(inventory: StoreInventoryItemRaw[]): WikiStoreCardViewModel[] {
    const cards: WikiStoreCardViewModel[] = [];

    for (const item of inventory) {
      if (CardRepository.getCardById(item.cardId) === undefined) {
        continue;
      }

      cards.push(WikiStoreCardConverter.convert(item.cardId, item.price));
    }

    return cards;
  }
}
