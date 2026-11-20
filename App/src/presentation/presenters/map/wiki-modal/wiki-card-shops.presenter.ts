import type { Quest } from "@/state/models";
import { QuestService } from "@/presentation/services/quest.service";
import type { CardShopViewModel } from "@/presentation/viewmodels/card/card-shop.viewmodel";

export class WikiCardShopsPresenter {
  public static getCardShopsAccordingMainQuest(
    cardShops: CardShopViewModel[],
    mainQuest: Quest,
  ): CardShopViewModel[] {
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);

    return cardShops.filter((shop) => {
      return QuestService.isOnMainQuestRange(
        lastCompletedMainQuestStep,
        shop.mainQuestAvailabilityWindow,
      );
    });
  }
}
