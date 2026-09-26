import type { TechniqueType } from "@/types/technique-type.type";

export interface TechniqueRaw {
  type: TechniqueType;
  element: string;
  elementStrength: number;
  mp: number;
  power: number;
}
