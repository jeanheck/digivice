import type { DigievolutionRequirement } from "@/types/digievolution-requirement.type";

export interface RequirementViewModel {
  type: DigievolutionRequirement;
  digievolution?: number;
  stat?: string;
  value: number;
}
