import { WikiCardShopConverter } from "@/presenters/converter/wiki-card-shop.converter";
import { MainQuestRangeHelper } from "@/presenters/helper/main-quest-range.helper";
import type { Quest } from "@/models";
import { CardRepository } from "@/repositories/card.repository";
import { QuestService } from "@/services/quest.service";
import type { CardShopViewModel } from "@/viewmodels/card/card-shop.viewmodel";

export class WikiCardShopsPresenter {
  public static getViewModel(cardId: string, mainQuest: Quest | null): CardShopViewModel[] {
    const cardRaw = CardRepository.getCardById(cardId);
    if (cardRaw === undefined) {
      return [];
    }

    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const activeStoreIds = new Set<string>();
    const result: CardShopViewModel[] = [];

    for (const store of cardRaw.stores ?? []) {
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

      const storeViewModel = WikiCardShopConverter.convert(store.storeId);
      if (storeViewModel === null) {
        continue;
      }

      activeStoreIds.add(store.storeId);
      result.push(storeViewModel);
    }

    return result;
  }
}
