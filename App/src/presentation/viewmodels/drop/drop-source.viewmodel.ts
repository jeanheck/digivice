import type { DropSourceKind } from "@/shared/types/drop-source-kind.type";

export interface DropSourceViewModel {
  kind: DropSourceKind;
  sourceId: string;
  labelKey?: string;
  label?: string;
  locationId?: string;
  ownerNameKey?: string;
}
