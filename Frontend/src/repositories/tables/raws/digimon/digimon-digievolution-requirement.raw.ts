import type { DigievolutionRequirement } from "@/types/digievolution-requirement.type";

export interface DigimonDigievolutionRequirementRaw {
  type: DigievolutionRequirement;
  digievolution?: number;
  stat?: string;
  value: number;
}
