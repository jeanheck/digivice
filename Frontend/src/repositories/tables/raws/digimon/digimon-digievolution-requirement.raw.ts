import type { DigievolutionRequirementConstant } from "@/constants/digievolution-requirement.constant";

export interface DigimonDigievolutionRequirementRaw {
    type: DigievolutionRequirementConstant;
    digievolution?: number;
    stat?: string;
    value: number;
}
