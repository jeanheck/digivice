import type { WikiEnemyDropViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop.viewmodel";
import type { WikiNpcDeckCardViewModel } from "@/viewmodels/wiki-modal/wiki-npc-deck-card.viewmodel";

export interface WikiNpcCardBattleViewModel {
  nameKey: string;
  level: number;
  cards: WikiNpcDeckCardViewModel[];
  drops: WikiEnemyDropViewModel[];
}
