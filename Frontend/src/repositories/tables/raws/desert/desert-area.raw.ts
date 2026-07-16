import type { DesertAreaTypeRaw } from "./desert-area-type.raw";

export interface DesertAreaRaw {
  label: string;
  type: DesertAreaTypeRaw;
  note?: string;
}
