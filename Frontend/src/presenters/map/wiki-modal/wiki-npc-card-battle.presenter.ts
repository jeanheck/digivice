import { WikiNpcDeckCardConverter } from "@/presenters/converter/wiki-npc-deck-card.converter";
import { CardRepository } from "@/repositories/card.repository";
import { DeckRepository } from "@/repositories/deck.repository";
import { NpcRepository } from "@/repositories/npc.repository";
import type { WikiNpcCardBattleViewModel } from "@/viewmodels/wiki-modal/wiki-npc-card-battle.viewmodel";
import type { WikiNpcDeckCardViewModel } from "@/viewmodels/wiki-modal/wiki-npc-deck-card.viewmodel";

export class WikiNpcCardBattlePresenter {
  public static getBattleViewModel(
    npcId: string,
    battleIndex: number,
  ): WikiNpcCardBattleViewModel | null {
    const npcRaw = NpcRepository.getNpcById(npcId);
    const cardBattle = npcRaw?.cardBattles?.[battleIndex];
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
