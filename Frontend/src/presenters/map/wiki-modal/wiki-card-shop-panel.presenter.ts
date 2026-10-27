import type { CardInventory, Quest } from "@/models";
import { CardShopRepository } from "@/repositories/card-shop.repository";
import type { CardShopPhaseRaw } from "@/repositories/tables/raws/tcg/card-shop.raw";
import { QuestService } from "@/services/quest.service";
import type { WikiCardShopInventoryCardViewModel } from "@/viewmodels/wiki-modal/wiki-card-shop-inventory-card.viewmodel";
import type { WikiCardShopViewModel } from "@/viewmodels/wiki-modal/wiki-card-shop.viewmodel";

export class WikiCardShopPanelPresenter {
  public static getViewModel(
    cardShopId: string,
    mainQuest: Quest,
    cardInventory?: CardInventory,
  ): WikiCardShopViewModel {
    const cardShopRaw = CardShopRepository.getById(cardShopId);
    if (cardShopRaw === undefined) {
      return {
        cards: [],
        locationId: null,
      };
    }

    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);

    return {
      cards: this.getActiveInventory(
        cardShopRaw.phases,
        lastCompletedMainQuestStep,
        cardInventory,
      ),
      locationId: cardShopRaw.locationId,
    };
  }

  private static getActiveInventory(
    phases: CardShopPhaseRaw[],
    lastCompletedMainQuestStep: number,
    cardInventory?: CardInventory,
  ): WikiCardShopInventoryCardViewModel[] {
    const inventory: WikiCardShopInventoryCardViewModel[] = [];

    for (const phase of phases) {
      const isInRange = QuestService.isOnMainQuestRange(
        lastCompletedMainQuestStep,
        phase.mainQuestAvailabilityWindow,
      );
      if (!isInRange) {
        continue;
      }

      for (const item of phase.inventory) {
        inventory.push({
          cardId: item.cardId,
          price: item.price,
          quantityOwned: cardInventory?.cards[item.cardId] ?? 0,
        });
      }
    }

    return inventory;
  }
}
