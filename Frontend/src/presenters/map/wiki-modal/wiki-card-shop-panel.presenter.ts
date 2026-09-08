import { WikiCardShopInventoryCardConverter } from "@/presenters/converter/wiki-card-shop-inventory-card.converter";
import type { Quest } from "@/models";
import { CardRepository } from "@/repositories/card.repository";
import { CardShopRepository } from "@/repositories/card-shop.repository";
import type {
  CardShopInventoryItemRaw,
  CardShopPhaseRaw,
} from "@/repositories/tables/raws/tcg/card-shop.raw";
import { QuestService } from "@/services/quest.service";
import type { WikiCardShopInventoryCardViewModel } from "@/viewmodels/wiki-modal/wiki-card-shop-inventory-card.viewmodel";
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
    const inventory = this.getActiveInventory(
      cardShopRaw.phases,
      lastCompletedMainQuestStep,
    );

    return {
      cards: this.getInventoryCards(inventory),
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

  private static getInventoryCards(
    inventory: CardShopInventoryItemRaw[],
  ): WikiCardShopInventoryCardViewModel[] {
    const cards: WikiCardShopInventoryCardViewModel[] = [];

    for (const item of inventory) {
      if (CardRepository.getCardById(item.cardId) === undefined) {
        continue;
      }

      cards.push(WikiCardShopInventoryCardConverter.convert(item.cardId, item.price));
    }

    return cards;
  }
}
