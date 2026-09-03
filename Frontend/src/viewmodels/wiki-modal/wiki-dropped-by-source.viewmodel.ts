import type { DropSourceKind } from "@/viewmodels/drop/drop-source.viewmodel";

export interface WikiDroppedBySourceViewModel {
  kind: DropSourceKind;
  sourceId: string;
  labelKey?: string;
  label?: string;
  iconUrl: string | null;
  locationId?: string;
}
