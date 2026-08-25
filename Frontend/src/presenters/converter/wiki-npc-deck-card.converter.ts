import type { DeckCardRaw } from "@/repositories/tables/raws/tcg/deck.raw";
import type { WikiNpcDeckCardViewModel } from "@/viewmodels/wiki-modal/wiki-npc-deck-card.viewmodel";

export class WikiNpcDeckCardConverter {
  public static convert(deckCardRaw: DeckCardRaw): WikiNpcDeckCardViewModel {
    return {
      cardId: deckCardRaw.id,
      nameKey: `cards.${deckCardRaw.id}.name`,
      quantity: deckCardRaw.quantity,
    };
  }
}
