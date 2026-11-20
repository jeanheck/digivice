import type { DropSourceKind } from "@/shared/types/drop-source-kind.type";

export interface WikiDroppedBySourceViewModel {
  kind: DropSourceKind;
  sourceId: string;
  labelKey?: string;
  label?: string;
  iconUrl: string | null;
  locationId?: string;
  ownerNameKey?: string;
}
