import type { DropType } from "@/presentation/repositories/tables/raws/drop/drop-type";
import type { WikiDroppedBySourceViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-dropped-by-source.viewmodel";

export interface WikiDropsPanelViewModel {
  dropType: DropType;
  dropNumericId: number;
  sources: WikiDroppedBySourceViewModel[];
  sourcesSectionLabelKey: string;
  sourcesEmptyLabelKey: string;
  weaponShopIds: string[];
}
