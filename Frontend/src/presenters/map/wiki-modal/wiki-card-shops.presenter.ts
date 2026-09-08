import { WikiCardShopConverter } from "@/presenters/converter/wiki-card-shop.converter";
import { MainQuestRangeHelper } from "@/presenters/helper/main-quest-range.helper";
import type { Quest } from "@/models";
import { QuestService } from "@/services/quest.service";
import type { CardShopSourceViewModel } from "@/viewmodels/card/card-shop-source.viewmodel";
import type { CardShopViewModel } from "@/viewmodels/card/card-shop.viewmodel";

export class WikiCardShopsPresenter {
  public static getViewModel(
    cardShops: CardShopSourceViewModel[],
    mainQuest: Quest | null,
  ): CardShopViewModel[] {
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const activeCardShopIds = new Set<string>();
    const result: CardShopViewModel[] = [];

    for (const shop of cardShops) {
      if (activeCardShopIds.has(shop.id)) {
        continue;
      }

      const isInRange = MainQuestRangeHelper.isInMainQuestRange(
        shop.startWhenLastMainQuestStepDone,
        shop.finishWhenLastMainQuestStepDone,
        lastCompletedMainQuestStep,
      );
      if (!isInRange) {
        continue;
      }

      const cardShopViewModel = WikiCardShopConverter.convert(shop.id);
      if (cardShopViewModel === null) {
        continue;
      }

      activeCardShopIds.add(shop.id);
      result.push(cardShopViewModel);
    }

    return result;
  }
}
