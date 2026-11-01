import type { TechniqueType } from "@/shared/types/technique-type.type";

export type DnaDigievolutionEffect = "reviveAndFullHealAll" | "strengthDown";

export interface DnaDigievolutionRaw {
  name: string;
  order: number;
  digievolutions: [number, number][];
  type: TechniqueType;
  element: string;
  elementStrength: number;
  mp: number;
  power: number;
  effect: DnaDigievolutionEffect | null;
}
