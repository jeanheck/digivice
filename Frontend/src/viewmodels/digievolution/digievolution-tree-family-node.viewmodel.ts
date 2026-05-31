import type { DigimonDigievolutionRequirementViewModel } from "../digimon/digimon-digievolution-requirement.viewmodel";

export interface DigievolutionTreeFamilyNodeViewModel {
    id: number;
    name: string;
    next: number | number[] | null;
    requirements: DigimonDigievolutionRequirementViewModel[];
}
