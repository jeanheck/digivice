import type { MobiusDesertAreaTypeRaw } from "./mobius-desert-area-type.raw";

export interface MobiusDesertAreaRaw {
  label: string;
  type: MobiusDesertAreaTypeRaw;
  note?: string;
}
