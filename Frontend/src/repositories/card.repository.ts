import CardsJson from "@/database/tcg/cards.json";
import type { CardsTable } from "@/repositories/tables/tcg/cards.table";
import type { CardRaw } from "@/repositories/tables/raws/tcg/card.raw";

export class CardRepository {
  private static readonly cardsTable = CardsJson as CardsTable;

  public static getCardById(cardId: string): CardRaw | undefined {
    return this.cardsTable[cardId];
  }

  public static getCardsTable(): CardsTable {
    return this.cardsTable;
  }

  public static getCardIds(): string[] {
    return Object.keys(this.cardsTable);
  }
}
