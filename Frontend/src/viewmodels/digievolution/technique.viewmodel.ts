import type { TechniqueType } from "@/types/technique-type.type";

export interface TechniqueViewModel {
  id: string;
  learnAt: number;
  loadAt: number | null;
  type: TechniqueType;
  element: string;
  elementStrength: number;
  mp: number;
  power: number;
  isUnlocked: boolean;
  isSignature: boolean;
}
