import { WikiNpcDeckCardConverter } from "@/presenters/converter/wiki-npc-deck-card.converter";
import { CardRepository } from "@/repositories/card.repository";
import { DeckRepository } from "@/repositories/deck.repository";
import { TamerRepository } from "@/repositories/tamer.repository";
import type { WikiNpcCardBattleViewModel } from "@/viewmodels/wiki-modal/wiki-npc-card-battle.viewmodel";
import type { WikiNpcDeckCardViewModel } from "@/viewmodels/wiki-modal/wiki-npc-deck-card.viewmodel";

export class WikiNpcCardBattlePresenter {
  public static getBattleViewModel(
    npcId: string,
    battleId: string,
  ): WikiNpcCardBattleViewModel | null {
    const tamerRaw = TamerRepository.getTamerById(npcId);
    const cardBattle = tamerRaw?.cardBattles?.[battleId];
    if (cardBattle === undefined) {
      return null;
    }

    const deckRaw = DeckRepository.getDeckById(cardBattle.deckId);
    if (deckRaw === undefined) {
      return null;
    }

    const cards: WikiNpcDeckCardViewModel[] = [];

    for (const deckCardRaw of deckRaw.cards) {
      if (CardRepository.getCardById(deckCardRaw.id) === undefined) {
        continue;
      }

      cards.push(WikiNpcDeckCardConverter.convert(deckCardRaw));
    }

    return {
      nameKey: `deck.${cardBattle.deckId}`,
      level: deckRaw.level,
      cards,
      drops: [
        {
          id: cardBattle.dropId,
        },
      ],
    };
  }
}
