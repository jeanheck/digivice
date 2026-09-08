import { WikiCardShopInventoryCardConverter } from "@/presenters/converter/wiki-card-shop-inventory-card.converter";
import type { Quest } from "@/models";
import { CardRepository } from "@/repositories/card.repository";
import { CardShopRepository } from "@/repositories/card-shop.repository";
import type {
  CardShopInventoryItemRaw,
  CardShopPhaseRaw,
} from "@/repositories/tables/raws/tcg/card-shop.raw";
import { NpcService } from "@/services/npc.service";
import { QuestService } from "@/services/quest.service";
import type { WikiCardShopInventoryCardViewModel } from "@/viewmodels/wiki-modal/wiki-card-shop-inventory-card.viewmodel";
import type { WikiCardShopPanelViewModel } from "@/viewmodels/wiki-modal/wiki-card-shop-panel.viewmodel";

export class WikiCardShopPanelPresenter {
  public static getViewModel(cardShopId: string, mainQuest: Quest | null): WikiCardShopPanelViewModel {
    const cardShopRaw = CardShopRepository.getById(cardShopId);
    if (cardShopRaw === undefined) {
      return {
        cards: [],
        locationId: null,
      };
    }

    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const inventory = WikiCardShopPanelPresenter.getActiveInventory(
      cardShopRaw.phases,
      lastCompletedMainQuestStep,
    );

    return {
      cards: WikiCardShopPanelPresenter.getInventoryCards(inventory),
      locationId: cardShopRaw.locationId,
    };
  }

  private static getActiveInventory(
    phases: CardShopPhaseRaw[],
    lastCompletedMainQuestStep: number,
  ): CardShopInventoryItemRaw[] {
    const inventory: CardShopInventoryItemRaw[] = [];

    for (const phase of phases) {
      const isInRange = NpcService.isVisibleOnMapByMainQuestStep(
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
