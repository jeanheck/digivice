import { CardRepository } from "@/repositories/card.repository";
import type { DeckCardRaw } from "@/repositories/tables/raws/tcg/deck.raw";
import type { WikiNpcDeckCardViewModel } from "@/viewmodels/wiki-modal/wiki-npc-deck-card.viewmodel";

export class WikiNpcDeckCardConverter {
  public static convert(deckCardRaw: DeckCardRaw): WikiNpcDeckCardViewModel {
    const cardRaw = CardRepository.getCardById(deckCardRaw.id);

    return {
      cardId: deckCardRaw.id,
      imageName: cardRaw?.imageName ?? "",
      nameKey: `cards.${deckCardRaw.id}.name`,
      quantity: deckCardRaw.quantity,
    };
  }
}
