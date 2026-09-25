import type { DropType } from "@/repositories/tables/raws/drop/drop-type";
import type { WikiDroppedBySourceViewModel } from "@/viewmodels/wiki-modal/wiki-dropped-by-source.viewmodel";

export interface WikiDropsPanelViewModel {
  dropType: DropType;
  dropNumericId: number;
  sources: WikiDroppedBySourceViewModel[];
  sourcesSectionLabelKey: string;
  sourcesEmptyLabelKey: string;
}
