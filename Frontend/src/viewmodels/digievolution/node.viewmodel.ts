import type { RequirementViewModel as RequirementViewModel } from "../digimon/requirement.viewmodel";

export interface NodeViewModel {
    id: number;
    name: string;
    next: number | number[] | null;
    requirements: RequirementViewModel[];
}
