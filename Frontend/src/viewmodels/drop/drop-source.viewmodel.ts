export type DropSourceKind = "enemy" | "tamer" | "duelIsland";

export interface DropSourceViewModel {
  kind: DropSourceKind;
  sourceId: string;
  labelKey?: string;
  label?: string;
  locationId?: string;
}
