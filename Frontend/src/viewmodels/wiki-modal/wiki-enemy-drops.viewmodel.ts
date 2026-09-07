import type { WikiEnemyDropItemViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop-item.viewmodel";

export interface WikiEnemyDropsViewModel {
  sectionLabelKey: string;
  fallbackLabelKey: string;
  hasInteractiveDrops: boolean;
  drops: WikiEnemyDropItemViewModel[];
}
