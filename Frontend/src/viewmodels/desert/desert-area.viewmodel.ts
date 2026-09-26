import type { MobiusDesertAreaType } from "@/types/mobius-desert-area-type.type";

export interface DesertAreaViewModel {
  label: string;
  type: MobiusDesertAreaType;
  note?: string;
}
