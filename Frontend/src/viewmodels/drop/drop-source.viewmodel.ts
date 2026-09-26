import type { DropSourceKind } from "@/types/drop-source-kind.type";

export interface DropSourceViewModel {
  kind: DropSourceKind;
  sourceId: string;
  labelKey?: string;
  label?: string;
  locationId?: string;
  tamerId?: string;
}
