import type { Quest } from "@/models";
import { CardShopRepository } from "@/repositories/card-shop.repository";
import type {
  CardShopInventoryItemRaw,
  CardShopPhaseRaw,
} from "@/repositories/tables/raws/tcg/card-shop.raw";
import { QuestService } from "@/services/quest.service";
import type { WikiCardShopViewModel } from "@/viewmodels/wiki-modal/wiki-card-shop.viewmodel";

export class WikiCardShopPanelPresenter {
  public static getViewModel(cardShopId: string, mainQuest: Quest | null): WikiCardShopViewModel {
    const cardShopRaw = CardShopRepository.getById(cardShopId);
    if (cardShopRaw === undefined) {
      return {
        cards: [],
        locationId: null,
      };
    }

    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);

    return {
      cards: this.getActiveInventory(cardShopRaw.phases, lastCompletedMainQuestStep),
      locationId: cardShopRaw.locationId,
    };
  }

  private static getActiveInventory(
    phases: CardShopPhaseRaw[],
    lastCompletedMainQuestStep: number,
  ): CardShopInventoryItemRaw[] {
    const inventory: CardShopInventoryItemRaw[] = [];

    for (const phase of phases) {
      const isInRange = QuestService.isOnMainQuestRange(
        lastCompletedMainQuestStep,
        phase.mainQuestStepDone,
      );
      if (!isInRange) {
        continue;
      }

      inventory.push(...phase.inventory);
    }

    return inventory;
  }
}
