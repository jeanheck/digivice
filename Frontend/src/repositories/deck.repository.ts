import DeckJson from "@/database/tcg/deck.json";
import type { DeckTable } from "@/repositories/tables/tcg/deck.table";
import type { DeckRaw } from "@/repositories/tables/raws/tcg/deck.raw";

export class DeckRepository {
  private static readonly deckTable = DeckJson as DeckTable;

  public static getDeckById(deckId: string): DeckRaw | undefined {
    return this.deckTable[deckId];
  }

  public static getDeckTable(): DeckTable {
    return this.deckTable;
  }
}
