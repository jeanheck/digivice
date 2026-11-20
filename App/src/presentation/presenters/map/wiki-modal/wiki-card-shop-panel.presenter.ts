import type { Quest } from "@/state/models";
import { CardRepository } from "@/presentation/repositories/card.repository";
import { CardShopRepository } from "@/presentation/repositories/card-shop.repository";
import type { CardShopPhaseRaw } from "@/presentation/repositories/tables/raws/tcg/card-shop.raw";
import { QuestService } from "@/presentation/services/quest.service";
import type { WikiCardShopInventoryCardViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-card-shop-inventory-card.viewmodel";
import type { WikiCardShopViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-card-shop.viewmodel";

export class WikiCardShopPanelPresenter {
  public static getViewModel(cardShopId: string, mainQuest: Quest): WikiCardShopViewModel {
    const cardShopRaw = CardShopRepository.getById(cardShopId);
    if (cardShopRaw === undefined) {
      return {
        cards: [],
        locationId: null,
      };
    }

    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);

    return {
      cards: this.getActiveInventory(cardShopRaw.phases, lastCompletedMainQuestStep)
        .map((cardId) => {
          return this.getCard(cardId);
        })
        .filter((card): card is WikiCardShopInventoryCardViewModel => {
          return card !== null;
        }),
      locationId: cardShopRaw.locationId,
    };
  }

  private static getActiveInventory(
    phases: CardShopPhaseRaw[],
    lastCompletedMainQuestStep: number,
  ): string[] {
    const inventory: string[] = [];

    for (const phase of phases) {
      const isInRange = QuestService.isOnMainQuestRange(
        lastCompletedMainQuestStep,
        phase.mainQuestAvailabilityWindow,
      );
      if (!isInRange) {
        continue;
      }

      inventory.push(...phase.inventory);
    }

    return inventory;
  }

  private static getCard(cardId: string): WikiCardShopInventoryCardViewModel | null {
    const cardRaw = CardRepository.getCardById(cardId);
    if (cardRaw?.price === undefined) {
      return null;
    }

    return {
      cardId,
      price: cardRaw.price,
    };
  }
}
