import CardJson from "@/data/game/tcg/card.json";
import type { CardTable } from "@/presentation/repositories/tables/tcg/card.table";
import type { CardRaw } from "@/presentation/repositories/tables/raws/tcg/card.raw";

export class CardRepository {
  private static readonly cardTable = CardJson as CardTable;

  public static getCardById(cardId: string): CardRaw | undefined {
    return this.cardTable[cardId];
  }

  public static getCardTable(): CardTable {
    return this.cardTable;
  }

  public static getCardIds(): string[] {
    return Object.keys(this.cardTable);
  }
}
