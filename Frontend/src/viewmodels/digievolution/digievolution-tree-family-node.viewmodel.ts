import type { DigimonDigievolutionRequirementViewModel } from "../digimon/digimon-digievolution-requirement.viewmodel";

export interface DigievolutionTreeFamilyNodeViewModel {
    name: string;
    before: string | null;
    next: string | string[] | null;
    requirements: DigimonDigievolutionRequirementViewModel[];
}