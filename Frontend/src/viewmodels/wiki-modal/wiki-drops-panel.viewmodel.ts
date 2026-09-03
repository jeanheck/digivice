import type { DropType } from "@/repositories/tables/raws/drop/drop.raw";
import type { WikiDroppedBySourceViewModel } from "@/viewmodels/wiki-modal/wiki-dropped-by-source.viewmodel";

export interface WikiDropsPanelViewModel {
  dropType: DropType | null;
  dropNumericId: number | null;
  sources: WikiDroppedBySourceViewModel[];
  sourcesSectionLabelKey: string;
  sourcesEmptyLabelKey: string;
}
