import type { DigievolutionRequirement } from "@/shared/types/digievolution-requirement.type";

export interface RequirementViewModel {
  type: DigievolutionRequirement;
  digievolution?: number;
  stat?: string;
  value: number;
}
