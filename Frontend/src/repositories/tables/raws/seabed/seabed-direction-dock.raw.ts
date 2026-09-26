import type { LabelPlacement } from "@/types/label-placement.type";

export type SeabedDirectionDockType = "normal" | "dead-end";

export interface SeabedDirectionDockRaw {
  location: string;
  x: number;
  y: number;
  type: SeabedDirectionDockType;
  labelPlacement: LabelPlacement;
}
