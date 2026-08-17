import type { WikiDroppedByEnemyViewModel } from "@/viewmodels/wiki-modal/wiki-dropped-by-enemy.viewmodel";

export interface WikiDropsPanelViewModel {
  dropType: string | null;
  sources: WikiDroppedByEnemyViewModel[];
}
