import type { WikiProfileDropItemViewModel } from "@/viewmodels/wiki-modal/wiki-profile-drop-item.viewmodel";

export interface WikiProfileDropsViewModel {
  sectionLabelKey: string;
  fallbackLabelKey: string;
  hasInteractiveDrops: boolean;
  drops: WikiProfileDropItemViewModel[];
}
