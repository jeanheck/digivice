import type { TechniqueType } from "@/types/technique-type.type";

export interface TechniqueViewModel {
  id: string;
  learnLevel: number;
  loadedLevel: number | null;
  type: TechniqueType;
  element: string;
  elementStrength: number;
  mp: number;
  power: number;
  isUnlocked: boolean;
  isSignature: boolean;
}
