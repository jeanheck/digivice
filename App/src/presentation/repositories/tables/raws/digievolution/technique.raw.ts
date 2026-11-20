import type { TechniqueType } from "@/shared/types/technique-type.type";

export interface TechniqueRaw {
  type: TechniqueType;
  element: string;
  elementStrength: number;
  mp: number;
  power: number;
}
