import DeckJson from "@/database/tcg/deck.json";
import type { DecksTable } from "@/repositories/tables/tcg/decks.table";
import type { DeckRaw } from "@/repositories/tables/raws/tcg/deck.raw";

export class DeckRepository {
  private static readonly decksTable = DeckJson as DecksTable;

  public static getDeckById(deckId: string): DeckRaw | undefined {
    return this.decksTable[deckId];
  }

  public static getDeckTable(): DecksTable {
    return this.decksTable;
  }
}
