import type { DropSourceKind } from "@/types/drop-source-kind.type";

export interface BoosterDroppedByRaw {
  kind: DropSourceKind;
  id: string;
  locationOnly?: string;
}

export interface BoosterRaw {
  droppedBy?: BoosterDroppedByRaw[];
}
