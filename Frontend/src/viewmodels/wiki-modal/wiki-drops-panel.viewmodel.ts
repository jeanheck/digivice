import type { DropType } from "@/repositories/tables/raws/drop/drop.raw";
import type { WikiDroppedByEnemyViewModel } from "@/viewmodels/wiki-modal/wiki-dropped-by-enemy.viewmodel";

export interface WikiDropsPanelViewModel {
  dropType: DropType | null;
  dropNumericId: number | null;
  sources: WikiDroppedByEnemyViewModel[];
}
