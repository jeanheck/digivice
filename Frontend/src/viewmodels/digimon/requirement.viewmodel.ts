import type { DigievolutionRequirementConstant } from "@/constants/digievolution-requirement.constant";

export interface RequirementViewModel {
    type: DigievolutionRequirementConstant;
    digievolution?: number;
    stat?: string;
    value: number;
}
