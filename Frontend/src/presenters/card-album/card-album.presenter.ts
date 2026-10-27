import type { CardInventory } from "@/models";
import { CardRepository } from "@/repositories/card.repository";
import type {
  CardAlbumItemViewModel,
  CardAlbumPageViewModel,
} from "@/viewmodels/card-album/card-album.viewmodel";

export class CardAlbumPresenter {
  public static readonly CARDS_PER_PAGE = 12;
  public static readonly TOTAL_PAGES = 27;
  public static readonly TOTAL_CARDS = 314;

  public static getPageViewModel(
    page: number,
    cardInventory?: CardInventory,
  ): CardAlbumPageViewModel {
    const validPage = Math.max(1, Math.min(page, this.TOTAL_PAGES));
    const cardTable = CardRepository.getCardTable();
    const sortedCardIds = CardRepository.getCardIds().sort(
      (a, b) => Number(a) - Number(b),
    );

    let totalUniqueOwned = 0;
    let totalCopies = 0;

    for (const cardId of sortedCardIds) {
      const qty = cardInventory?.cards[cardId] ?? 0;
      if (qty > 0) {
        totalUniqueOwned++;
        totalCopies += qty;
      }
    }

    const startIndex = (validPage - 1) * this.CARDS_PER_PAGE;
    const pageCardIds = sortedCardIds.slice(startIndex, startIndex + this.CARDS_PER_PAGE);

    const slots: (CardAlbumItemViewModel | null)[] = [];

    for (let i = 0; i < this.CARDS_PER_PAGE; i++) {
      const cardId = pageCardIds[i];
      if (cardId === undefined) {
        slots.push(null);
        continue;
      }

      const cardRaw = cardTable[cardId];
      if (cardRaw === undefined) {
        slots.push(null);
        continue;
      }

      const quantityOwned = cardInventory?.cards[cardId] ?? 0;
      const cardIndex = Number(cardId) - 999;

      slots.push({
        cardId,
        cardIndex,
        imageName: cardRaw.imageName,
        nameKey: `card.${cardId}.name`,
        noteKey: `card.${cardId}.note`,
        type: cardRaw.type,
        points: cardRaw.points,
        quantityOwned,
        isOwned: quantityOwned > 0,
      });
    }

    return {
      page: validPage,
      totalPages: this.TOTAL_PAGES,
      cards: slots,
      totalUniqueOwned,
      totalCards: sortedCardIds.length,
      totalCopies,
    };
  }
}
