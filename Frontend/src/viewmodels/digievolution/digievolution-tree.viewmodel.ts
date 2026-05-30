import type { DigimonDigievolutionRequirementViewModel } from "@/viewmodels/digimon/digimon-digievolution-requirement.viewmodel";

export interface DigievolutionTreeNodeViewModel {
    name: string;
    before: string | null;
    next: string | string[] | null;
    requirements: DigimonDigievolutionRequirementViewModel[];
}

export type DigievolutionsTreeViewModel = Record<string, DigievolutionTreeNodeViewModel[]>
