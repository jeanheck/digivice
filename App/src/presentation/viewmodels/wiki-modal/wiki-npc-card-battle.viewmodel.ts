import type { EnemyDropViewModel } from "@/presentation/viewmodels/enemy/enemy-drop.viewmodel";
import type { WikiNpcFolderCardViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-npc-folder-card.viewmodel";

export interface WikiNpcCardBattleViewModel {
  nameKey: string;
  level: number;
  cards: WikiNpcFolderCardViewModel[];
  drops: EnemyDropViewModel[];
}
