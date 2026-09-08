import { MainQuestRangeHelper } from "@/presenters/helper/main-quest-range.helper";
import type { Quest } from "@/models";
import { QuestService } from "@/services/quest.service";
import type { CardShopViewModel } from "@/viewmodels/card/card-shop.viewmodel";

export class WikiCardShopsPresenter {
  public static getCardShopsAccordingMainQuest(
    cardShops: CardShopViewModel[],
    mainQuest: Quest | null,
  ): CardShopViewModel[] {
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);

    return cardShops.filter((shop) => {
      return MainQuestRangeHelper.isInMainQuestRange(
        shop.startWhenLastMainQuestStepDone,
        shop.finishWhenLastMainQuestStepDone,
        lastCompletedMainQuestStep,
      );
    });
  }
}
